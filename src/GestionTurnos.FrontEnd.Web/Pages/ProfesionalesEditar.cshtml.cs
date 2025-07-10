using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionTurnos.FrontEnd.Web.Models;
using GestionTurnos.FrontEnd.Web.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class ProfesionalesEditarModel : PageModel
    {
        private readonly ProfesionalApiService _profesionalApi;
        private readonly ServicioApiService _servicioApi;
        [BindProperty]
        public Profesional Profesional { get; set; } = new();
        public SelectList ServiciosSelect { get; set; } = null!;
        public string? Mensaje { get; set; }

        public ProfesionalesEditarModel(ProfesionalApiService profesionalApi, ServicioApiService servicioApi)
        {
            _profesionalApi = profesionalApi;
            _servicioApi = servicioApi;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var profesional = await _profesionalApi.ObtenerProfesional(id);
            if (profesional == null) return RedirectToPage("/Profesionales");
            Profesional = profesional;
            var servicios = await _servicioApi.ObtenerServicios();
            ServiciosSelect = new SelectList(servicios, "Id", "Nombre");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var servicios = await _servicioApi.ObtenerServicios();
            ServiciosSelect = new SelectList(servicios, "Id", "Nombre");
            var result = await _profesionalApi.EditarProfesional(id, Profesional);
            Mensaje = result ? "Profesional actualizado correctamente." : "Error al actualizar profesional.";
            if (result) return RedirectToPage("/Profesionales");
            return Page();
        }
    }
}
