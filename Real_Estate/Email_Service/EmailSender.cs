using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace Real_Estate.Email_Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("wm1336562@gmail.com", "ocrk mmkw elsy prxi")
            };
            return client.SendMailAsync(new MailMessage(from: "wm1336562@gmail.com", to: email, subject, htmlMessage) { IsBodyHtml = true });
        }
    }
}
