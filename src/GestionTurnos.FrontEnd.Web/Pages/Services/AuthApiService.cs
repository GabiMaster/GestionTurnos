using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GestionTurnos.FrontEnd.Web.Models;

namespace GestionTurnos.FrontEnd.Web.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _http;

        public AuthApiService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://localhost:7087/api/"); // Cambiar si tu API usa otro puerto
        }

        public async Task<string?> Login(string email, string password)
        {
            var response = await _http.PostAsJsonAsync("auth/login", new { email, password });

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<JwtResponse>();
            return result?.Token;
        }

        public class JwtResponse
        {
            public string Token { get; set; } = "";
        }

        public async Task<bool> Register(Usuario usuario)
        {
            // El backend espera PasswordHash, no Password
            var payload = new
            {
                usuario.NombreCompleto,
                usuario.Email,
                PasswordHash = usuario.Password, // El backend lo hashea
                Rol = 0 // Puedes ajustar el rol si es necesario
            };
            var response = await _http.PostAsJsonAsync("auth/register", payload);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RecuperarPassword(string email)
        {
            var response = await _http.PostAsJsonAsync("auth/recuperar-password", new { email });
            return response.IsSuccessStatusCode;
        }
    }
}

