using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Blog.Models.Extensions
{
    public class MyEmailService
    {
        public string SmtpHost = "smtp.mailtrap.io";
        public int SmtpPort = 2525;
        public string SmtpUsername = "c2716a1025a38a";
        public string SmtpPassword = "31a4dc60c81fae";
        public string SmtpSender = "Admin@bugtracker.com";

        public void Send(string receiver, string subject, string body)
        {
            MailMessage message = new MailMessage(SmtpSender, receiver);

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient(SmtpHost, SmtpPort);

            smtpClient.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);

            smtpClient.EnableSsl = true;

            smtpClient.Send(message);
        }
    }
}