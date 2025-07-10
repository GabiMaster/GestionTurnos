namespace GestionTurnos.BackEnd.Model.Entities
{
    public class Profesional
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
        public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
    }
}
