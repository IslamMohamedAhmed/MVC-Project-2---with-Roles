using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Utilities
{
    public class Email
    {
        public string Subject { get; set; }
        public string Body { get; set; }

        public string Recipent { get; set; }
    }
    public static class SentEmailSettings
    {
        public static async Task SendEmailAsync(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com",587);

            client.EnableSsl = true;

            client.Credentials = new NetworkCredential("nanorules714@gmail.com", "pfzsnpgacaxezsyq");

            await client.SendMailAsync("nanorules714@gmail.com",email.Recipent,email.Subject,email.Body);
        }

    }
}
