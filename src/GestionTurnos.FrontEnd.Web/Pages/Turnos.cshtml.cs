using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestionTurnos.FrontEnd.Web.Models;
using GestionTurnos.FrontEnd.Web.Services;
using Microsoft.AspNetCore.Http;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class TurnosModel : PageModel
    {
        private readonly ServicioApiService _servicioApi;
        private readonly TurnoApiService _turnoApi;
        private readonly ProfesionalApiService _profesionalApi;

        public List<Turno> Turnos { get; set; } = new();
        public List<Servicio> Servicios { get; set; } = new();
        public List<Profesional> Profesionales { get; set; } = new();
        public SelectList ServiciosSelect => new(Servicios, "Id", "Nombre");
        public SelectList ProfesionalesSelect => new(Profesionales, "Id", "NombreCompleto");

        [BindProperty]
        public Turno NuevoTurno { get; set; } = new();
        public string? Mensaje { get; set; }
        public bool RequiereLogin { get; set; } = false;

        public TurnosModel(ServicioApiService servicioApi, TurnoApiService turnoApi)
        {
            _servicioApi = servicioApi;
            _turnoApi = turnoApi;
            _profesionalApi = profesionalApi;
        }

        public async Task OnGetAsync()
        {
            var token = HttpContext.Session.GetString("JWT");
            if (string.IsNullOrEmpty(token))
            {
                RequiereLogin = true;
                return;
            }
            Servicios = await _servicioApi.ObtenerServicios();
            Profesionales = await _profesionalApi.ObtenerProfesionales();
            Turnos = await _turnoApi.ObtenerTurnosUsuario();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("JWT");
            if (string.IsNullOrEmpty(token))
            {
                RequiereLogin = true;
                return Page();
            }
            Servicios = await _servicioApi.ObtenerServicios();
            Profesionales = await _profesionalApi.ObtenerProfesionales();
            var resultado = await _turnoApi.AgendarTurno(NuevoTurno);
            if (resultado)
                Mensaje = "Turno agendado correctamente.";
            else
                Mensaje = "No se pudo agendar el turno.";
            Turnos = await _turnoApi.ObtenerTurnosUsuario();
            return Page();
        }

        public async Task<IActionResult> OnPostCancelarAsync(int id)
        {
            await _turnoApi.CancelarTurno(id);
            return RedirectToPage();
        }
    }
}
