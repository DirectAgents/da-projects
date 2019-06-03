namespace CakeExtracter.Common.Email
{
    /// <summary>
    /// Email Notifications Service.
    /// </summary>
    public interface IEmailNotificationsService
    {
        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <typeparam name="T">Type of notification model.</typeparam>
        /// <param name="to">To emails array.</param>
        /// <param name="copy">The copy emails array.</param>
        /// <param name="model">The notification model.</param>
        /// <param name="templateName">Name of the email template.</param>
        void SendEmail<T>(string[] to, string[] copy, T model, string templateName);
    }
}
