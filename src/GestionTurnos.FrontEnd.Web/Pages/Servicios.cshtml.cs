using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionTurnos.FrontEnd.Web.Models;
using GestionTurnos.FrontEnd.Web.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class ServiciosModel : PageModel
    {
        private readonly ServicioApiService _servicioApi;

        public List<Servicio> ListadoServicios { get; set; } = new();
        public bool RequiereLogin { get; set; } = false;
        public bool EsAdmin { get; set; } = false;
        public string? Mensaje { get; set; }

        public ServiciosModel(ServicioApiService servicioApi)
        {
            _servicioApi = servicioApi;
        }

        public async Task OnGetAsync()
        {
            var token = HttpContext.Session.GetString("JWT");
            if (string.IsNullOrEmpty(token))
            {
                RequiereLogin = true;
                return;
            }
            // Leer rol del usuario desde los claims del JWT
            var rol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            EsAdmin = rol == "Administrador";
            ListadoServicios = await _servicioApi.ObtenerServicios();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            var ok = await _servicioApi.EliminarServicio(id);
            Mensaje = ok ? "Servicio eliminado correctamente." : "No se pudo eliminar el servicio.";
            ListadoServicios = await _servicioApi.ObtenerServicios();
            return Page();
        }
    }
}

