using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionTurnos.FrontEnd.Web.Models;
using GestionTurnos.FrontEnd.Web.Services;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class ServiciosEditarModel : PageModel
    {
        private readonly ServicioApiService _servicioApi;
        [BindProperty]
        public Servicio Servicio { get; set; } = new();
        public string? Mensaje { get; set; }

        public ServiciosEditarModel(ServicioApiService servicioApi)
        {
            _servicioApi = servicioApi;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var servicios = await _servicioApi.ObtenerServicios();
            Servicio = servicios.FirstOrDefault(s => s.Id == id) ?? new Servicio();
            if (Servicio.Id == 0) return RedirectToPage("/Servicios");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var ok = await _servicioApi.EditarServicio(id, Servicio);
            Mensaje = ok ? "Servicio actualizado correctamente." : "No se pudo actualizar el servicio.";
            if (ok) return RedirectToPage("/Servicios");
            return Page();
        }
    }
}
