using CoreAdminLTE.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;

namespace CoreAdminLTE.Services
{
    public class EmailService : IEmailService, IDisposable
    {
        private readonly IConfiguration configuration;
        private readonly SmtpClient smtpClient;
        public EmailService(
            IConfiguration configuration
        )
        {
            this.configuration = configuration;
            smtpClient = new SmtpClient();

        }

        public void SendMail(EmailModel emailModel)
        {
            
            smtpClient.Host = configuration["Email:Host"]; // "mail.yandex.com"
            smtpClient.Credentials = new System.Net.NetworkCredential(configuration["Email:UserName"], configuration["Email:Password"]);
            int port = 587;
            int.TryParse(configuration["Email:Port"], out port);
            smtpClient.Port = port;
            smtpClient.EnableSsl = true;
            
            MailMessage mailMessage = new MailMessage(configuration["Email:From"], emailModel.To, emailModel.Subject, emailModel.Body);
            mailMessage.BodyEncoding =  System.Text.Encoding.UTF8;

            smtpClient.Send(mailMessage);

            
        }

        public void Dispose()
        {
            smtpClient.Dispose();
        }
    }
}