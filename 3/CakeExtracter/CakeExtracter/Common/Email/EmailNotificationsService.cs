using System;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using RazorEngine;
using RazorEngine.Templating;
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
        /// <param name="bodyTemplateName">Name of the body template.</param>
        /// <param name="subjectTemplateName">Name of the subject template.</param>
        public void SendEmail<T>(string[] to, string[] copy, T model, string bodyTemplateName, string subjectTemplateName)
        {
            var client = GetAuthenticatedSendGridClient();
            var msg = InitMessage(to, copy, model, bodyTemplateName, subjectTemplateName);
            var response = client.SendEmailAsync(msg).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                throw new ApplicationException("Error occurred while sending email");
            }
        }

        private SendGridMessage InitMessage<T>(string[] to, string[] copy, T model, string bodyTemplateName, string subjectTemplateName)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(GetEmailFromAddress()),
                Subject = PrepareEmailTextContent(model, subjectTemplateName),
                HtmlContent = PrepareEmailTextContent(model, bodyTemplateName),
            };
            to.Select(email => new EmailAddress(email)).ForEach(emailAddress => msg.AddTo(emailAddress));
            copy.Select(email => new EmailAddress(email)).ForEach(emailAddress => msg.AddCc(emailAddress));
            return msg;
        }

        private string PrepareEmailTextContent<T>(T model, string templateName)
        {
            var templateRelativePath = $"Email\\{templateName}.html";
            var templateFileContent = FileUtils.GetFileContentByRelativePath(templateRelativePath);
            var result = Engine.Razor.RunCompile(templateFileContent, templateName, null, model);
            return result;
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

        private string GetEmailFromAddress()
        {
            const string fromEmailConfigKey = "JEM_Failure_FromEmail";
            var fromEmail = ConfigurationManager.AppSettings[fromEmailConfigKey];
            if (string.IsNullOrEmpty(fromEmail))
            {
                throw new ApplicationException("From email should be defined in the configuration.");
            }
            return fromEmail;
        }
    }
}
