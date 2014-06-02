using System;
using System.Net.Mail;
using System.Linq;

namespace DAgents.Common
{
    public static class EmailUtility
    {
        public static void SendEmail(string from, string to, string cc, string subject, string body, bool isHTML)
        {
            string[] toArray = (to == null) ? new string[] { } : new string[] { to };
            string[] ccArray = (cc == null) ? new string[] { } : new string[] { cc };
            SendEmail(from, toArray, ccArray, subject, body, isHTML);
        }

        public static void SendEmail(string from, string[] to, string[] cc, string subject, string body, bool isHTML)
        {
            MailMessage message = new MailMessage
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = isHTML,
                From = new MailAddress(from),
            };
            foreach (var item in to.SelectMany(c => c.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)))
                message.To.Add(item);
            Array.ForEach(cc, c => message.CC.Add(c));
            SmtpClient SmtpMailer = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Timeout = 50000,
                EnableSsl = true
            };
            SmtpMailer.Credentials = new System.Net.NetworkCredential("reporting@directagents.com", "1423qrwe");
            SmtpMailer.Send(message);
        }
    }
}
