using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using GestionTurnos.FrontEnd.Web.Services;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly AuthApiService _authApi;
        public string? Mensaje { get; set; }

        public ProfileModel(AuthApiService authApi)
        {
            _authApi = authApi;
        }

        public void OnGet() { }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("JWT");
            return RedirectToPage("/Login");
        }

        public async Task<IActionResult> OnPostResetPassword()
        {
            var email = HttpContext.Session.GetString("Email");
            var token = Request.Form["Token"].ToString();
            var nuevaPassword = Request.Form["NuevaPassword"].ToString();
            var confirmarPassword = Request.Form["ConfirmarPassword"].ToString();
            if (nuevaPassword != confirmarPassword)
            {
                Mensaje = "Las contraseñas no coinciden.";
                return Page();
            }
            var result = await _authApi.ResetPasswordToken(email, token, nuevaPassword);
            Mensaje = result ? "Contraseña actualizada correctamente." : "Error al actualizar la contraseña.";
            return Page();
        }
    }
}
