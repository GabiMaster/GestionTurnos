namespace GestionTurnos.BackEnd.Model.Entities
{
    public class Profesional
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string Especialidad { get; set; } = null!;
        public ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
        public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    }
}
