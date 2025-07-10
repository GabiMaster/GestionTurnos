using Microsoft.AspNetCore.Mvc;
using GestionTurnos.BackEnd.ServiceDependencies.Interfaces;
using GestionTurnos.BackEnd.Data.Contexts;
using Shared.DTO;
using GestionTurnos.BackEnd.Model.Dto;
using Microsoft.EntityFrameworkCore;

namespace GestionTurnos.BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly AppDbContext _db;

        public AuthController(IAuthService authService, AppDbContext db)
        {
            _authService = authService;
            _db = db;
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
            return Ok();
        }

        public class RecuperarPasswordRequest
        {
            public string Email { get; set; } = null!;
        }
    }
}



