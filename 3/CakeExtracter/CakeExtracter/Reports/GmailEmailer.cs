using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

// TODO: separate project
namespace CakeExtracter.Reports
{
    public class GmailEmailer
    {
        public NetworkCredential Credential { get; set; }

        public GmailEmailer(NetworkCredential credential)
        {
            this.Credential = credential;
        }

        private void SendMailMessage(MailMessage message)
        {
            var client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Timeout = 50000,
                EnableSsl = true,
                Credentials = this.Credential
            };
            client.Send(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddresses">An array of strings representing destination addresses.  Each string may optionally be a comma separated list.</param>
        /// <param name="ccAddresses"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="isHTML"></param>
        public void SendEmail(string fromAddress, string[] toAddresses, string[] ccAddresses, string subject, string body, bool isHTML)
        {
            var message = CreateMailMessage(fromAddress, toAddresses, ccAddresses, subject);
            message.Body = body;
            message.IsBodyHtml = isHTML;

            SendMailMessage(message);
        }

        public void SendEmail(string fromAddress, string[] toAddresses, string[] ccAddresses, string subject, AlternateView[] alternateViews)
        {
            var message = CreateMailMessage(fromAddress, toAddresses, ccAddresses, subject);
            foreach (var alternateView in alternateViews)
            {
                message.AlternateViews.Add(alternateView);
            }
            SendMailMessage(message);
        }


        private MailMessage CreateMailMessage(string fromAddress, string[] toAddresses, string[] ccAddresses, string subject)
        {
            MailMessage message = new MailMessage
            {
                Subject = subject,
                From = new MailAddress(fromAddress),
            };

            foreach (var item in toAddresses.SelectMany(c => c.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)))
            {
                message.To.Add(item.Trim());
            }

            if (ccAddresses != null)
            {
                foreach (var ccAddress in ccAddresses.SelectMany(c => c.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)))
                {
                    message.CC.Add(ccAddress);
                }
            }
            return message;
        }

    }
}
