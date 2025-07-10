namespace GestionTurnos.BackEnd.Model.Entities
{
    public class Turno
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public bool Confirmado { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public int ServicioId { get; set; }
        public Servicio Servicio { get; set; } = null!;

        public int ProfesionalId { get; set; }
        public Profesional Profesional { get; set; } = null!;

        public string? TokenQR { get; set; }
        public DateTime? FechaExpiracionQR { get; set; }
    }
}
