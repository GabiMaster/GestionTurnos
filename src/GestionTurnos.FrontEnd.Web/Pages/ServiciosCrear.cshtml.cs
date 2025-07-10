using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionTurnos.FrontEnd.Web.Models;
using GestionTurnos.FrontEnd.Web.Services;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class ServiciosCrearModel : PageModel
    {
        private readonly ServicioApiService _servicioApi;
        [BindProperty]
        public Servicio Servicio { get; set; } = new();
        public string? Mensaje { get; set; }

        public ServiciosCrearModel(ServicioApiService servicioApi)
        {
            _servicioApi = servicioApi;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var (ok, error) = await _servicioApi.CrearServicioConError(Servicio);
            Mensaje = ok ? "Servicio creado correctamente." : $"No se pudo crear el servicio. {error}";
            if (ok) return RedirectToPage("/Servicios");
            return Page();
        }
    }
}
