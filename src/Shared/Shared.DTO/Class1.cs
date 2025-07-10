namespace Shared.DTO
{
    public class ServicioDTO
    {

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int DuracionMinutos { get; set; }
    }
    public class ProfesionalDTO
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public List<int> ServiciosIds { get; set; } = new();
    }
    public class TurnoDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ServicioId { get; set; }
        public int ProfesionalId { get; set; }
        public DateTime FechaHora { get; set; }
        public bool Confirmado { get; set; }
        public string? TokenQR { get; set; }
        public DateTime? FechaExpiracionQR { get; set; }
    }
}

