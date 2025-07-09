using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionTurnos.FrontEnd.Web.Services;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class RecuperarPasswordModel : PageModel
    {
        private readonly AuthApiService _authService;
        [BindProperty]
        public string Email { get; set; } = string.Empty;
        public string? Mensaje { get; set; }

        public RecuperarPasswordModel(AuthApiService authService)
        {
            _authService = authService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var ok = await _authService.RecuperarPassword(Email);
            Mensaje = ok ? "Si el email existe, se ha enviado un enlace de recuperación." : "No se pudo procesar la solicitud.";
            return Page();
        }
    }
}
