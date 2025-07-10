using Microsoft.AspNetCore.Mvc;
using GestionTurnos.BackEnd.Model.Entities;
using Microsoft.EntityFrameworkCore;
using GestionTurnos.BackEnd.Data.Contexts;
using Microsoft.AspNetCore.Authorization;

namespace GestionTrunos.BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiciosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiciosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize] // Solo requiere estar autenticado
        public async Task<ActionResult<IEnumerable<Servicio>>> Get()
        {
            return await _context.Servicios.ToListAsync();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<Servicio>> Create(Servicio servicio)
        {
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = servicio.Id }, servicio);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update(int id, Servicio servicio)
        {
            var entity = await _context.Servicios.FindAsync(id);
            if (entity == null) return NotFound();
            entity.Nombre = servicio.Nombre;
            entity.DuracionMinutos = servicio.DuracionMinutos;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Servicios.FindAsync(id);
            if (entity == null) return NotFound();
            _context.Servicios.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

