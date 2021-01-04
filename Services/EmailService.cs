using CoreAdminLTE.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;

namespace CoreAdminLTE.Services
{
    public class EmailService : IEmailService, IDisposable
    {
        public IConfiguration Configuration { get; }
        public EmailService(
            IConfiguration Configuration
        )
        {
            this.Configuration = Configuration;

        }

        private SmtpClient smtpClient;
        public void SendMail(EmailModel emailModel)
        {
            smtpClient = new SmtpClient();
            smtpClient.Host = Configuration["Email:Host"]; // "mail.yandex.com"
            smtpClient.Credentials = new System.Net.NetworkCredential(Configuration["Email:UserName"], Configuration["Email:Password"]);
            int port = 587;
            int.TryParse(Configuration["Email:Port"], out port);
            smtpClient.Port = port;
            smtpClient.EnableSsl = true;
            
            MailMessage mailMessage = new MailMessage(Configuration["Email:From"], emailModel.To, emailModel.Subject, emailModel.Body);
            mailMessage.BodyEncoding =  System.Text.Encoding.UTF8;

            smtpClient.Send(mailMessage);

            
        }

        public void Dispose()
        {
            smtpClient.Dispose();
        }
    }
}