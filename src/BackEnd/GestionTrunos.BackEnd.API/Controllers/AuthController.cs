using Microsoft.AspNetCore.Http;
using GestionTurnos.BackEnd.Model.Entities;
using GestionTurnos.BackEnd.ServiceDependencies.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using GestionTurnos.BackEnd.Data.Contexts;

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
        public IActionResult Register([FromBody] Usuario usuario)
        {
            if (_db.Usuarios.Any(u => u.Email == usuario.Email))
                return BadRequest("El email ya está registrado");

            usuario.PasswordHash = _authService.HashPassword(usuario.PasswordHash, out var salt);
            usuario.PasswordSalt = salt;

            _db.Usuarios.Add(usuario);
            _db.SaveChanges();

            return Ok("Usuario registrado");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _db.Usuarios.SingleOrDefault(u => u.Email == request.Email);
            if (user == null)
                return Unauthorized("Usuario no encontrado");

            var isValid = _authService.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt);
            if (!isValid)
                return Unauthorized("Contraseña incorrecta");

            var token = _authService.GenerateJwtToken(user);
            return Ok(new { token });
        }

        [HttpPost("recuperar-password")]
        public IActionResult RecuperarPassword([FromBody] RecuperarPasswordRequest request)
        {
            // Simulación: en un caso real, deberías enviar un email con un enlace de recuperación
            var user = _db.Usuarios.SingleOrDefault(u => u.Email == request.Email);
            if (user == null)
                return Ok(); // No revelar si el email existe
            // Aquí deberías generar un token y enviarlo por email
            // Por ahora, solo simula éxito
            return Ok();
        }

        public class LoginRequest
        {
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
        }

        public class RecuperarPasswordRequest
        {
            public string Email { get; set; } = null!;
        }
    }
}

