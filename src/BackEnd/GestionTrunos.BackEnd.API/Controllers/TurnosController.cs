using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionTurnos.BackEnd.Data.Contexts;
using GestionTurnos.BackEnd.Model.Entities;
using Shared.DTO;
using System.Security.Claims;

namespace GestionTrunos.BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurnosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TurnosController(AppDbContext context)
        {
            _context = context;
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

        // POST: api/turnos/agendar
        [HttpPost("agendar")]
        public async Task<IActionResult> AgendarTurno([FromBody] TurnoDTO dto)
        {
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
                UsuarioId = dto.UsuarioId,
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
    }
}
