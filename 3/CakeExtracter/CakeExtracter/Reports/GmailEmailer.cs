using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

// TODO: separate project
namespace CakeExtracter.Reports
{
    public class GmailEmailer
    {
        public GmailEmailer(NetworkCredential credential)
        {
            this.Credential = credential;
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
            MailMessage message = new MailMessage
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = isHTML,
                From = new MailAddress(fromAddress),
            };

            foreach (var item in toAddresses.SelectMany(c => c.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)))
            {
                message.To.Add(item.Trim());
            }

            if (ccAddresses != null)
            {
                foreach (var ccAddress in ccAddresses)
                {
                    message.CC.Add(ccAddress);
                }
            }

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

        public NetworkCredential Credential { get; set; }
    }
}
