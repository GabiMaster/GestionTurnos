using GestionTurnos.FrontEnd.Web.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GestionTurnos.FrontEnd.Web.Services
{
    public class ServicioApiService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _context;

        public ServicioApiService(HttpClient http, IHttpContextAccessor context)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://localhost:7087/api/");
            _context = context;
        }

        private void SetAuthorizationHeader()
        {
            var token = _context.HttpContext?.Session.GetString("JWT");
            _http.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(token))
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<Servicio>> ObtenerServicios()
        {
            SetAuthorizationHeader();
            var result = await _http.GetFromJsonAsync<List<Servicio>>("servicios");
            return result ?? new List<Servicio>();
        }

        public async Task<bool> CrearServicio(Servicio servicio)
        {
            SetAuthorizationHeader();
            var response = await _http.PostAsJsonAsync("servicios", servicio);
            return response.IsSuccessStatusCode;
        }

        public async Task<(bool ok, string? error)> CrearServicioConError(Servicio servicio)
        {
            SetAuthorizationHeader();
            var response = await _http.PostAsJsonAsync("servicios", servicio);
            if (response.IsSuccessStatusCode)
                return (true, null);
            var error = await response.Content.ReadAsStringAsync();
            return (false, error);
        }

        public async Task<bool> EditarServicio(int id, Servicio servicio)
        {
            SetAuthorizationHeader();
            var response = await _http.PutAsJsonAsync($"servicios/{id}", servicio);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarServicio(int id)
        {
            SetAuthorizationHeader();
            var response = await _http.DeleteAsync($"servicios/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}

