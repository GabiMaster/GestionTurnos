using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionTurnos.BackEnd.Data.Contexts;
using GestionTurnos.BackEnd.Model.Entities;
using Shared.DTO;
using System.Security.Claims;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GestionTrunos.BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurnosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public TurnosController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: api/turnos/mis-turnos
        [HttpGet("mis-turnos")]
        public async Task<ActionResult<IEnumerable<Turno>>> GetMisTurnos()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized();
            var turnos = await _context.Turnos
                .Include(t => t.Servicio)
                .Include(t => t.Profesional)
                .Where(t => t.UsuarioId == userId)
                .ToListAsync();
            return turnos;
        }

        // GET: api/turnos/validar-qr?token=...
        [HttpGet("validar-qr")]
        public async Task<IActionResult> ValidarQR([FromQuery] string token)
        {
            var turno = await _context.Turnos
                .Include(t => t.Servicio)
                .Include(t => t.Profesional)
                .FirstOrDefaultAsync(t => t.TokenQR == token);
            if (turno == null || turno.FechaExpiracionQR < DateTime.UtcNow)
                return BadRequest("QR inválido o expirado");
            // Aquí podrías marcar asistencia si lo deseas
            return Ok(new {
                turno.Id,
                turno.FechaHora,
                turno.Confirmado,
                turno.Servicio,
                turno.Profesional,
                turno.FechaExpiracionQR
            });
        }

        // POST: api/turnos/agendar
        [HttpPost("agendar")]
        public async Task<IActionResult> AgendarTurno([FromBody] TurnoDTO dto)
        {
            // Obtener el id del usuario autenticado
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized();

            // Validar solapamiento
            var profesionalTurnos = await _context.Turnos
                .Where(t => t.ProfesionalId == dto.ProfesionalId &&
                            t.FechaHora < dto.FechaHora.AddMinutes(30) &&
                            t.FechaHora.AddMinutes(30) > dto.FechaHora)
                .ToListAsync();
            if (profesionalTurnos.Any())
                return BadRequest("El profesional ya tiene un turno en ese horario.");

            var turno = new Turno
            {
                UsuarioId = userId, // Usar el id autenticado
                ServicioId = dto.ServicioId,
                ProfesionalId = dto.ProfesionalId,
                FechaHora = dto.FechaHora,
                Confirmado = true, // Confirmado por defecto
                TokenQR = Guid.NewGuid().ToString(),
                FechaExpiracionQR = DateTime.UtcNow.AddHours(2)
            };
            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();
            // TODO: Enviar email de confirmación
            return Ok(turno);
        }

        // DELETE: api/turnos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarTurno(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null) return NotFound();
            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: api/turnos/confirmar-asistencia?token=...
        [HttpPatch("confirmar-asistencia")]
        public async Task<IActionResult> ConfirmarAsistencia([FromQuery] string token)
        {
            var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.TokenQR == token);
            if (turno == null || turno.FechaExpiracionQR < DateTime.UtcNow)
                return BadRequest("QR inválido o expirado");
            if (turno.Confirmado)
                return BadRequest("La asistencia ya fue confirmada.");
            turno.Confirmado = true;
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Asistencia confirmada", turno.Id, turno.FechaHora });
        }

        private string GenerarQRBase64(string url)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var svgQr = new SvgQRCode(qrData);
            return svgQr.GetGraphic(4);
        }
    }
}
