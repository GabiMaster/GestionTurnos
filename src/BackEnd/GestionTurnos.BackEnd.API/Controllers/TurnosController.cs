using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionTurnos.BackEnd.Data.Contexts;
using GestionTurnos.BackEnd.Model.Entities;
using Shared.DTO;
using System.Security.Claims;
using System.IO;
using ZXing;
using ZXing.QrCode;
using System;

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
        public async Task<ActionResult<IEnumerable<object>>> GetMisTurnos()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized();
            var turnos = await _context.Turnos
                .Include(t => t.Servicio)
                .Include(t => t.Profesional)
                .Where(t => t.UsuarioId == userId)
                .ToListAsync();
            var baseUrl = _config["FrontendUrl"] ?? "https://localhost:7087";
            var apiBase = Request.Scheme + "://" + Request.Host.Value;
            var result = turnos.Select(t => new {
                t.Id,
                t.FechaHora,
                t.Confirmado,
                t.UsuarioId,
                t.ServicioId,
                t.ProfesionalId,
                t.Servicio,
                t.Profesional,
                t.FechaExpiracionQR,
                TokenQR = t.TokenQR != null ? $"{apiBase}/api/turnos/qr-image?token={t.TokenQR}" : null
            });
            return Ok(result);
        }

        // GET: api/turnos/qr-image?token=...
        [HttpGet("qr-image")]
        public IActionResult GetQRImage([FromQuery] string token)
        {
            // Usar siempre la URL base del frontend para el QR, forzando el puerto 7298
            var baseUrl = _config["FrontendUrl"] ?? "https://localhost:7298";
            var url = $"{baseUrl}/ValidarQR?token={token}";
            var qrWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.QrCode.QrCodeEncodingOptions
                {
                    Height = 300,
                    Width = 300,
                    Margin = 1
                }
            };
            var pixelData = qrWriter.Write(url);
            using var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            try
            {
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
            using var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms.ToArray(), "image/png");
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
                FechaExpiracionQR = DateTime.UtcNow.AddHours(3) // Ahora expira en 3 horas
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
    }
}
