using Microsoft.AspNetCore.Mvc;
using GestionTurnos.BackEnd.ServiceDependencies.Interfaces;
using GestionTurnos.BackEnd.Data.Contexts;
using Shared.DTO;
using GestionTurnos.BackEnd.Model.Dto;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GestionTurnos.BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly AppDbContext _db;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;

        public AuthController(IAuthService authService, AppDbContext db, IEmailSender emailSender, IConfiguration config)
        {
            _authService = authService;
            _db = db;
            _emailSender = emailSender;
            _config = config;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUsuarioDTO dto)
        {
            if (_db.Usuarios.Any(u => u.Email == dto.Email))
                return BadRequest("El email ya está registrado");

            var passwordHash = _authService.HashPassword(dto.Password, out var salt);

            var usuario = new Usuario
            {
                NombreUsuario = dto.Usuario,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = salt,
                Rol = dto.Rol
            };

            _db.Usuarios.Add(usuario);
            _db.SaveChanges();

            return Ok("Usuario registrado");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "Email no encontrado" });
            }

            if (!_authService.VerifyPassword(dto.Password, usuario.PasswordHash, usuario.PasswordSalt))
            {
                return Unauthorized(new { mensaje = "Contraseña incorrecta" });
            }

            var token = _authService.GenerateJwtToken(usuario);
            return Ok(new { token });
        }

        [HttpPost("recuperar-password")]
        public IActionResult RecuperarPassword([FromBody] RecuperarPasswordRequest request)
        {
            var user = _db.Usuarios.SingleOrDefault(u => u.Email == request.Email);
            if (user == null)
                return Ok(); // No revelar si el email existe

            // Generar token seguro
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiration = DateTime.UtcNow.AddHours(2);
            _db.SaveChanges();

            var url = $"{_config["FrontendUrl"]}/ResetPassword?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";
            var body = $"<p>Para reestablecer tu contraseña haz clic en el siguiente enlace:</p><p><a href='{url}'>Reestablecer contraseña</a></p>";
            _emailSender.Send(user.Email, "Reestablecer contraseña", body);
            return Ok();
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest req)
        {
            var user = _db.Usuarios.SingleOrDefault(u => u.Email == req.Email && u.PasswordResetToken == req.Token && u.PasswordResetTokenExpiration > DateTime.UtcNow);
            if (user == null)
                return BadRequest("Token inválido o expirado");
            user.PasswordHash = _authService.HashPassword(req.NewPassword, out var salt);
            user.PasswordSalt = salt;
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiration = null;
            _db.SaveChanges();
            return Ok();
        }

        public class RecuperarPasswordRequest
        {
            public string Email { get; set; } = null!;
        }
        public class ResetPasswordRequest
        {
            public string Email { get; set; } = null!;
            public string Token { get; set; } = null!;
            public string NewPassword { get; set; } = null!;
        }
    }
}



