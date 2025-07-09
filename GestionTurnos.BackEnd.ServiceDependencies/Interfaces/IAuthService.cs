using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionTurnos.BackEnd.Model.Entities;

namespace GestionTurnos.BackEnd.ServiceDependencies.Interfaces
{
    public interface IAuthService
    {
        Usuario? Authenticate(string email, string password);
        string HashPassword(string password, out string salt);
        bool VerifyPassword(string password, string storedHash, string storedSalt);
        string GenerateJwtToken(Usuario usuario);
    }
}
