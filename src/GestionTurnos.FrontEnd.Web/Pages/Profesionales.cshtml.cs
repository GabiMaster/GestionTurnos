using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionTurnos.FrontEnd.Web.Models;
using GestionTurnos.FrontEnd.Web.Services;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class ProfesionalesModel : PageModel
    {
        private readonly ProfesionalApiService _profesionalApi;
        private readonly ServicioApiService _servicioApi;
        public List<ProfesionalViewModel> Profesionales { get; set; } = new();

        public ProfesionalesModel(ProfesionalApiService profesionalApi, ServicioApiService servicioApi)
        {
            _profesionalApi = profesionalApi;
            _servicioApi = servicioApi;
        }

        public async Task OnGetAsync()
        {
            var profesionales = await _profesionalApi.ObtenerProfesionales();
            var servicios = await _servicioApi.ObtenerServicios();
            Profesionales = profesionales.Select(p => new ProfesionalViewModel
            {
                Id = p.Id,
                NombreCompleto = p.NombreCompleto,
                Especialidad = p.Especialidad,
                ServiciosNombres = servicios.Where(s => p.ServiciosIds.Contains(s.Id)).Select(s => s.Nombre).ToList()
            }).ToList();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            await _profesionalApi.EliminarProfesional(id);
            return RedirectToPage();
        }

        public class ProfesionalViewModel
        {
            public int Id { get; set; }
            public string NombreCompleto { get; set; } = string.Empty;
            public string Especialidad { get; set; } = string.Empty;
            public List<string> ServiciosNombres { get; set; } = new();
        }
    }
}
