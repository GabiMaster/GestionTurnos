using System.Text.Json.Serialization;

namespace GestionTurnos.BackEnd.Model.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Rol
    {
        Administrador,
        Empleado,
        Cliente
    }
}

