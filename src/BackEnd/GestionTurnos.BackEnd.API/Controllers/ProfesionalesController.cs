using Microsoft.AspNetCore.Mvc;
using GestionTurnos.BackEnd.Model.Entities;
using Microsoft.EntityFrameworkCore;
using GestionTurnos.BackEnd.Data.Contexts;
using Shared.DTO;

namespace GestionTrunos.BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfesionalesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfesionalesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> Get()
        {
            var profesionales = await _context.Profesionales.Include(p => p.Servicios).ToListAsync();
            var result = profesionales.Select(p => new {
                p.Id,
                p.NombreCompleto,
                p.Especialidad,
                ServiciosIds = p.Servicios.Select(s => s.Id).ToList()
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Profesional>> GetById(int id)
        {
            var profesional = await _context.Profesionales.Include(p => p.Servicios).FirstOrDefaultAsync(p => p.Id == id);
            if (profesional == null) return NotFound();
            return profesional;
        }

        [HttpPost]
        public async Task<ActionResult<Profesional>> Create(ProfesionalDTO dto)
        {
            var servicios = await _context.Servicios.Where(s => dto.ServiciosIds.Contains(s.Id)).ToListAsync();
            var profesional = new Profesional
            {
                NombreCompleto = dto.NombreCompleto,
                Especialidad = dto.Especialidad,
                Servicios = servicios
            };
            _context.Profesionales.Add(profesional);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = profesional.Id }, profesional);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProfesionalDTO dto)
        {
            var profesional = await _context.Profesionales.Include(p => p.Servicios).FirstOrDefaultAsync(p => p.Id == id);
            if (profesional == null) return NotFound();
            profesional.NombreCompleto = dto.NombreCompleto;
            profesional.Especialidad = dto.Especialidad;
            profesional.Servicios = await _context.Servicios.Where(s => dto.ServiciosIds.Contains(s.Id)).ToListAsync();
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var profesional = await _context.Profesionales.FindAsync(id);
            if (profesional == null) return NotFound();
            _context.Profesionales.Remove(profesional);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
