using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionTurnos.FrontEnd.Web.Services;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly AuthApiService _authApi;
        [BindProperty]
        public string Email { get; set; } = string.Empty;
        [BindProperty]
        public string Token { get; set; } = string.Empty;
        public string? Mensaje { get; set; }
        public bool TokenInvalido { get; set; } = false;

        public ResetPasswordModel(AuthApiService authApi)
        {
            _authApi = authApi;
        }

        public void OnGet(string token, string email)
        {
            Token = token;
            Email = email;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var nuevaPassword = Request.Form["NuevaPassword"].ToString();
            var confirmarPassword = Request.Form["ConfirmarPassword"].ToString();
            if (nuevaPassword != confirmarPassword)
            {
                Mensaje = "Las contraseñas no coinciden.";
                return Page();
            }
            var ok = await _authApi.ResetPasswordToken(Email, Token, nuevaPassword);
            if (ok)
                Mensaje = "Contraseña actualizada correctamente. Ya puedes iniciar sesión.";
            else
                TokenInvalido = true;
            return Page();
        }
    }
}
