using GestionTurnos.FrontEnd.Web.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace GestionTurnos.FrontEnd.Web.Services
{
    public class ProfesionalApiService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _context;

        public ProfesionalApiService(HttpClient http, IHttpContextAccessor context)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://localhost:7087/api/");
            _context = context;
        }

        public async Task<List<Profesional>> ObtenerProfesionales()
        {
            var token = _context.HttpContext?.Session.GetString("JWT");
            if (!string.IsNullOrEmpty(token))
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await _http.GetFromJsonAsync<List<Profesional>>("profesionales");
            return result ?? new List<Profesional>();
        }

        public async Task<Profesional?> ObtenerProfesional(int id)
        {
            var token = _context.HttpContext?.Session.GetString("JWT");
            if (!string.IsNullOrEmpty(token))
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _http.GetFromJsonAsync<Profesional>($"profesionales/{id}");
        }

        public async Task<bool> CrearProfesional(Profesional profesional)
        {
            var token = _context.HttpContext?.Session.GetString("JWT");
            if (!string.IsNullOrEmpty(token))
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.PostAsJsonAsync("profesionales", profesional);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EditarProfesional(int id, Profesional profesional)
        {
            var token = _context.HttpContext?.Session.GetString("JWT");
            if (!string.IsNullOrEmpty(token))
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.PutAsJsonAsync($"profesionales/{id}", profesional);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarProfesional(int id)
        {
            var token = _context.HttpContext?.Session.GetString("JWT");
            if (!string.IsNullOrEmpty(token))
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.DeleteAsync($"profesionales/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
