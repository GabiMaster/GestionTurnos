using GestionTurnos.BackEnd.Model.Entities;

namespace GestionTurnos.BackEnd.ServiceDependencies.Interfaces
{
    public interface IAuthService
    {
        Usuario? Authenticate(string email, string password);
        string HashPassword(string password, out byte[] salt);
        bool VerifyPassword(string password, string storedHash, byte[] storedSalt);
        string GenerateJwtToken(Usuario usuario);
    }
}

