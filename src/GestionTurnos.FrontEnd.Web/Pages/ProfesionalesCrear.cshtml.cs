using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionTurnos.FrontEnd.Web.Models;
using GestionTurnos.FrontEnd.Web.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class ProfesionalesCrearModel : PageModel
    {
        private readonly ProfesionalApiService _profesionalApi;
        private readonly ServicioApiService _servicioApi;
        [BindProperty]
        public Profesional Profesional { get; set; } = new();
        public SelectList ServiciosSelect { get; set; } = null!;
        public string? Mensaje { get; set; }

        public ProfesionalesCrearModel(ProfesionalApiService profesionalApi, ServicioApiService servicioApi)
        {
            _profesionalApi = profesionalApi;
            _servicioApi = servicioApi;
        }

        public async Task OnGetAsync()
        {
            var servicios = await _servicioApi.ObtenerServicios();
            ServiciosSelect = new SelectList(servicios, "Id", "Nombre");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var servicios = await _servicioApi.ObtenerServicios();
            ServiciosSelect = new SelectList(servicios, "Id", "Nombre");
            var result = await _profesionalApi.CrearProfesional(Profesional);
            Mensaje = result ? "Profesional creado correctamente." : "Error al crear profesional.";
            if (result) return RedirectToPage("/Profesionales");
            return Page();
        }
    }
}
