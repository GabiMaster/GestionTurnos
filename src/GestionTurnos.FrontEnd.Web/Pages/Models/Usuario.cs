using GestionTurnos.BackEnd.Model.Entities;

namespace GestionTurnos.FrontEnd.Web.Models

{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public Rol Rol { get; set; }
        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}

