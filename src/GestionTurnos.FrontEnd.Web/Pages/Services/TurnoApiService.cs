using GestionTurnos.FrontEnd.Web.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GestionTurnos.FrontEnd.Web.Services
{
    public class TurnoApiService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _context;

        public TurnoApiService(HttpClient http, IHttpContextAccessor context)
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

        public async Task<List<Turno>> ObtenerTurnosUsuario()
        {
            SetAuthorizationHeader();
            var result = await _http.GetFromJsonAsync<List<Turno>>("turnos/mis-turnos");
            return result ?? new List<Turno>();
        }

        public async Task<bool> AgendarTurno(Turno turno)
        {
            SetAuthorizationHeader();
            var response = await _http.PostAsJsonAsync("turnos/agendar", turno);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CancelarTurno(int id)
        {
            SetAuthorizationHeader();
            var response = await _http.DeleteAsync($"turnos/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
