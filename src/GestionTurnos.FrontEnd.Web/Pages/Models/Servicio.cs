namespace GestionTurnos.FrontEnd.Web.Models
{
    public class Servicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public int DuracionMinutos { get; set; }
        // Opcional: lista de profesionales para mostrar en el frontend
        public List<Profesional>? Profesionales { get; set; }
    }
}

