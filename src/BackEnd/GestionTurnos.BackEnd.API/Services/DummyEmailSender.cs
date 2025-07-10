using GestionTurnos.BackEnd.ServiceDependencies.Interfaces;

namespace GestionTurnos.BackEnd.API.Services
{
    public class DummyEmailSender : IEmailSender
    {
        public void Send(string to, string subject, string body)
        {
            // Log para desarrollo
            Console.WriteLine($"[DummyEmailSender] To: {to}, Subject: {subject}, Body: {body}");
        }
    }
}
