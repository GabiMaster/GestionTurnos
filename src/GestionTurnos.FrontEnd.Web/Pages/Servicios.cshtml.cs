using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionTurnos.FrontEnd.Web.Models;
using GestionTurnos.FrontEnd.Web.Services;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class ServiciosModel : PageModel
    {
        private readonly ServicioApiService _servicioApi;

        public List<Servicio> ListadoServicios { get; set; } = new();

        public ServiciosModel(ServicioApiService servicioApi)
        {
            _servicioApi = servicioApi;
        }

        public async Task OnGetAsync()
        {
            ListadoServicios = await _servicioApi.ObtenerServicios();
        }
    }
}

