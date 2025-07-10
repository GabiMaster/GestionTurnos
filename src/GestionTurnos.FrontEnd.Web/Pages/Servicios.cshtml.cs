using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionTurnos.FrontEnd.Web.Models;
using GestionTurnos.FrontEnd.Web.Services;
using Microsoft.AspNetCore.Http;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class ServiciosModel : PageModel
    {
        private readonly ServicioApiService _servicioApi;

        public List<Servicio> ListadoServicios { get; set; } = new();
        public bool RequiereLogin { get; set; } = false;

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
            ListadoServicios = await _servicioApi.ObtenerServicios();
        }
    }
}

