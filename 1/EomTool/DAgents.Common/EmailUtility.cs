using System;
using System.Net.Mail;

namespace DAgents.Common
{
    public static class EmailUtility
    {
        public static void SendEmail(string from, string[] to, string[] cc, string subject, string body, bool isHTML)
        {
            MailMessage message = new MailMessage
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = isHTML,
                From = new MailAddress(from),
            };
            Array.ForEach(to, c => message.To.Add(c));
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
