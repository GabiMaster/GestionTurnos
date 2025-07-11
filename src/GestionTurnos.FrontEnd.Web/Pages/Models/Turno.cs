using System.Text.Json.Serialization;
namespace GestionTurnos.FrontEnd.Web.Models
{
    public class Turno
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public bool Confirmado { get; set; }
        public int UsuarioId { get; set; }
        public int ServicioId { get; set; }
        public int ProfesionalId { get; set; }
        public string? TokenQR { get; set; }
        public DateTime? FechaExpiracionQR { get; set; }
        public Servicio? Servicio { get; set; }
        public Profesional? Profesional { get; set; }
    }
}
