using GestionTurnos.BackEnd.Model.Entities;

namespace Shared.DTO
{
    public class RegisterUsuarioDTO
    {
        public string Usuario { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Rol Rol { get; set; }
    }
}

