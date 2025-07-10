using GestionTurnos.BackEnd.ServiceDependencies.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace GestionTurnos.BackEnd.API.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public SmtpEmailSender(IConfiguration config) { _config = config; }

        public void Send(string to, string subject, string body)
        {
            var smtp = new SmtpClient(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"]));
            smtp.Credentials = new NetworkCredential(_config["Smtp:User"], _config["Smtp:Pass"]);
            smtp.EnableSsl = true;
            var mail = new MailMessage(_config["Smtp:From"], to, subject, body) { IsBodyHtml = true };
            smtp.Send(mail);
        }
    }
}
