using GestionTurnos.FrontEnd.Web.Models;
using System.Net.Http.Headers;

namespace GestionTurnos.FrontEnd.Web.Services
{
    public class ServicioApiService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _context;

        public ServicioApiService(HttpClient http, IHttpContextAccessor context)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://localhost:44329/api/");
            _context = context;
        }

        public async Task<List<Servicio>> ObtenerServicios()
        {
            var token = _context.HttpContext?.Session.GetString("JWT");

            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var result = await _http.GetFromJsonAsync<List<Servicio>>("servicios");

            return result ?? new List<Servicio>();
        }
    }
}

