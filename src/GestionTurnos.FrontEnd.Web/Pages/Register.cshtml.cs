using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionTurnos.FrontEnd.Web.Services;
using GestionTurnos.FrontEnd.Web.Models;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly AuthApiService _authService;

        [BindProperty]
        public Usuario Usuario { get; set; } = new Usuario();
        public string? Mensaje { get; set; }
        public string? MensajeError { get; set; }

        public RegisterModel(AuthApiService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var (result, errorMsg) = await _authService.RegisterWithError(Usuario);
            Mensaje = result ? "Usuario registrado con éxito" : null;
            MensajeError = !result ? errorMsg : null;
            return Page();
        }
    }
}

