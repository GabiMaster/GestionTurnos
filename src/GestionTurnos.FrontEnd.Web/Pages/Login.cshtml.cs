using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionTurnos.FrontEnd.Web.Services;
using Microsoft.AspNetCore.Http;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AuthApiService _authService;

        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Password { get; set; }

        public string? MensajeError { get; set; }

        public LoginModel(AuthApiService authService)
        {
            _authService = authService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = await _authService.Login(Email, Password);

            if (string.IsNullOrEmpty(token))
            {
                MensajeError = "Credenciales incorrectas";
                return Page();
            }

            HttpContext.Session.SetString("JWT", token);
            return RedirectToPage("Servicios");
        }
    }
}

