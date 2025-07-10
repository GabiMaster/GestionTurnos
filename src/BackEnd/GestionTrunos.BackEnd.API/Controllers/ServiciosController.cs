using Microsoft.AspNetCore.Mvc;
using GestionTurnos.BackEnd.Model.Entities;
using Microsoft.EntityFrameworkCore;
using GestionTurnos.BackEnd.Data.Contexts;

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
        public async Task<ActionResult<IEnumerable<Servicio>>> Get()
        {
            return await _context.Servicios.ToListAsync();
        }
    }
}

