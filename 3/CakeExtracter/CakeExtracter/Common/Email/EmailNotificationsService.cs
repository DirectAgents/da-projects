using System;
using System.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CakeExtracter.Common.Email
{
    /// <summary>
    /// Email Notification Service (Uses Azure SendGrid for Email notifications).
    /// </summary>
    /// <seealso cref="CakeExtracter.Common.Email.IEmailNotificationsService" />
    public class EmailNotificationsService : IEmailNotificationsService
    {
        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <typeparam name="T">Type of notification model.</typeparam>
        /// <param name="to">To emails array.</param>
        /// <param name="copy">The copy emails array.</param>
        /// <param name="model">The notification model.</param>
        /// <param name="templateName">Name of the email template.</param>
        public void SendEmail<T>(string[] to, string[] copy, T model, string templateName)
        {
            var client = GetAuthenticatedSendGridClient();
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("test@example.com", "DX Team"),
                Subject = "Hello World from the SendGrid CSharp SDK!",
                PlainTextContent = "Hello, Email!",
                HtmlContent = "<strong>Hello, Email!</strong>"
            };
            msg.AddTo(new EmailAddress("o.harko@itransition.com", "Test User"));
            var response = client.SendEmailAsync(msg).Result;
        }

        private SendGridClient GetAuthenticatedSendGridClient()
        {
            const string sendGridApiKeyConfigKey = "AzureSendGridApiKey";
            var apiKey = ConfigurationManager.AppSettings[sendGridApiKeyConfigKey];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ApplicationException("Send Grid Api Key should be defined in the configuration.");
            }
            return new SendGridClient(apiKey);
        }
    }
}
