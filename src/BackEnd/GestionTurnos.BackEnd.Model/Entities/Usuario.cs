namespace GestionTurnos.BackEnd.Model.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string PasswordSalt { get; set; } = null!;
        public Rol Rol { get; set; } = Rol.Cliente;

        public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    }

}
