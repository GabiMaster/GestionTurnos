﻿using System.Net.Http;
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
            _http.BaseAddress = new Uri("https://localhost:7087/api/");
        }

        public async Task<string?> Login(string email, string password)
        {
            var response = await _http.PostAsJsonAsync("auth/login", new { email, password });

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<JwtResponse>();
            return result?.Token;
        }

        private class JwtResponse
        {
            public string Token { get; set; } = "";
        }

        public async Task<(bool, string?)> RegisterWithError(Usuario usuario)
        {
            var payload = new
            {
                Usuario = usuario.NombreUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Password = usuario.Password
            };
            var response = await _http.PostAsJsonAsync("auth/register", payload);
            if (response.IsSuccessStatusCode)
                return (true, null);
            var errorMsg = await response.Content.ReadAsStringAsync();
            return (false, errorMsg);
        }

        public async Task<bool> RecuperarPassword(string email)
        {
            var response = await _http.PostAsJsonAsync("auth/recuperar-password", new { email });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetPasswordToken(string email, string token, string newPassword)
        {
            var payload = new { email, token, newPassword };
            var response = await _http.PostAsJsonAsync("auth/reset-password", payload);
            return response.IsSuccessStatusCode;
        }
    }
}

