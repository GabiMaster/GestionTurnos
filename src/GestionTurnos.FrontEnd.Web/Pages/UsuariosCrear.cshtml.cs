using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionTurnos.FrontEnd.Web.Models;
using GestionTurnos.FrontEnd.Web.Services;
using Shared.DTO;

namespace GestionTurnos.FrontEnd.Web.Pages
{
    public class UsuariosCrearModel : PageModel
    {
        private readonly AuthApiService _authService;
        [BindProperty]
        public Usuario Usuario { get; set; } = new();
        public string? Mensaje { get; set; }

        public UsuariosCrearModel(AuthApiService authService)
        {
            _authService = authService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var (result, errorMsg) = await _authService.RegisterWithError(Usuario);
            Mensaje = result ? "Usuario creado correctamente." : errorMsg;
            if (result) return RedirectToPage("/Index");
            return Page();
        }
    }
}
