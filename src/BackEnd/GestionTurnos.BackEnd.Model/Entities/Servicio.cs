namespace GestionTurnos.BackEnd.Model.Entities
{
    public class Servicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int DuracionMinutos { get; set; }

        public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
        public ICollection<Profesional> Profesionales { get; set; } = new List<Profesional>();
    }
}
