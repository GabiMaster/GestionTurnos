using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestionTurnos.FrontEnd.Web.Models;
using GestionTurnos.FrontEnd.Web.Services;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class TurnosModel : PageModel
    {
        private readonly ServicioApiService _servicioApi;
        // Suponiendo que habrá un TurnoApiService para obtener/agendar turnos
        private readonly TurnoApiService _turnoApi;

        public List<Turno> Turnos { get; set; } = new();
        public List<Servicio> Servicios { get; set; } = new();
        public SelectList ServiciosSelect => new(Servicios, "Id", "Nombre");

        [BindProperty]
        public Turno NuevoTurno { get; set; } = new();
        public string? Mensaje { get; set; }

        public TurnosModel(ServicioApiService servicioApi, TurnoApiService turnoApi)
        {
            _servicioApi = servicioApi;
            _turnoApi = turnoApi;
        }

        public async Task OnGetAsync()
        {
            Servicios = await _servicioApi.ObtenerServicios();
            Turnos = await _turnoApi.ObtenerTurnosUsuario();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Servicios = await _servicioApi.ObtenerServicios();
            var resultado = await _turnoApi.AgendarTurno(NuevoTurno);
            if (resultado)
                Mensaje = "Turno agendado correctamente.";
            else
                Mensaje = "No se pudo agendar el turno.";
            Turnos = await _turnoApi.ObtenerTurnosUsuario();
            return Page();
        }
    }
}
