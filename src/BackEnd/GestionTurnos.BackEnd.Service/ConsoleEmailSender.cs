using GestionTurnos.BackEnd.ServiceDependencies.Interfaces;
using Microsoft.Extensions.Logging;

namespace GestionTurnos.BackEnd.Service
{
    public class ConsoleEmailSender : IEmailSender
    {
        private readonly ILogger<ConsoleEmailSender> _logger;
        public ConsoleEmailSender(ILogger<ConsoleEmailSender> logger)
        {
            _logger = logger;
        }
        public void Send(string to, string subject, string body)
        {
            _logger.LogInformation($"[EMAIL SIMULADO] To: {to}\nSubject: {subject}\nBody: {body}");
        }
    }
}
