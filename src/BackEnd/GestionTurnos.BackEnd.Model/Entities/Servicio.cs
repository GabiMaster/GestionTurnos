using System.Text.Json.Serialization;

namespace GestionTurnos.BackEnd.Model.Entities
{
    public class Servicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int DuracionMinutos { get; set; }

        [JsonIgnore]
        public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
        [JsonIgnore]
        public ICollection<Profesional> Profesionales { get; set; } = new List<Profesional>();
    }

}
