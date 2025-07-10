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
            var result = await _authService.Register(Usuario);
            Mensaje = result ? "Usuario creado correctamente." : "Error al crear usuario.";
            if (result) return RedirectToPage("/Index");
            return Page();
        }
    }
}
