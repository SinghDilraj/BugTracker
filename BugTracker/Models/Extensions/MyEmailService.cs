using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Blog.Models.Extensions
{
    public class MyEmailService
    {
        private readonly string SmtpHost = ConfigurationManager.AppSettings["SmtpHost"];
        private readonly int SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
        private readonly string SmtpUsername = ConfigurationManager.AppSettings["SmtpUsername"];
        private readonly string SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
        private readonly string SmtpFrom = ConfigurationManager.AppSettings["SmtpFrom"];

        public void Send(string receiver, string subject, string body)
        {
            MailMessage message = new MailMessage(SmtpFrom, receiver)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            SmtpClient smtpClient = new SmtpClient(SmtpHost, SmtpPort)
            {
                Credentials = new NetworkCredential(SmtpUsername, SmtpPassword),

                EnableSsl = true
            };

            smtpClient.Send(message);
        }
    }
}