using GestionTurnos.BackEnd.ServiceDependencies.Interfaces;
using Microsoft.Extensions.Logging;

namespace GestionTurnos.BackEnd.Service
{
    public class ConsoleEmailSender : IEmailSender
    {
        public ConsoleEmailSender(ILogger<ConsoleEmailSender> logger) { }
        public void Send(string to, string subject, string body) { }
    }
}
