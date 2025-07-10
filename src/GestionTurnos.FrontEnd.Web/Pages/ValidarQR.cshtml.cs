using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class ValidarQRModel : PageModel
    {
        public string? Mensaje { get; set; }
        public string? Error { get; set; }
        public string? Servicio { get; set; }
        public string? Profesional { get; set; }
        public DateTime? FechaHora { get; set; }

        public async Task OnGetAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                Error = "Token no proporcionado.";
                return;
            }
            try
            {
                using var http = new HttpClient();
                var response = await http.PatchAsync($"https://localhost:7087/api/turnos/confirmar-asistencia?token={token}", null);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);
                    Mensaje = doc.RootElement.GetProperty("mensaje").GetString();
                    FechaHora = doc.RootElement.TryGetProperty("fechaHora", out var fh) ? fh.GetDateTime() : (DateTime?)null;
                    // No se devuelven servicio/profesional en el PATCH, pero se podría extender
                }
                else
                {
                    Error = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Error = "Error al validar el QR: " + ex.Message;
            }
        }
    }
}
