namespace GestionTurnos.FrontEnd.Web.Models
{
    public class Turno
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public bool Confirmado { get; set; }
        public int UsuarioId { get; set; }
        public int ServicioId { get; set; }
        public string? TokenQR { get; set; }
        public Servicio? Servicio { get; set; }
    }
}
