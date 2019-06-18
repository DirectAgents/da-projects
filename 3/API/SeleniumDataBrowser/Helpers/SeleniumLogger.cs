using System;

namespace SeleniumDataBrowser.Helpers
{
    /// <summary>
    /// The logger for Selenium data browser.
    /// </summary>
    public class SeleniumLogger
    {
        private const string LoggerPrefix = "[Selenium]";

        private readonly Action<string> logInfo;
        private readonly Action<Exception> logError;
        private readonly Action<string> logWarning;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeleniumLogger"/> class.
        /// </summary>
        /// <param name="logInfo">Action for logging (info level).</param>
        /// <param name="logError">Action for logging (error level).</param>
        /// <param name="logWarning">Action for logging (warning level).</param>
        public SeleniumLogger(Action<string> logInfo, Action<Exception> logError, Action<string> logWarning)
        {
            this.logInfo = logInfo;
            this.logError = logError;
            this.logWarning = logWarning;
        }

        /// <summary>
        /// Log a message as info.
        /// </summary>
        /// <param name="message">Message for logging.</param>
        public void LogInfo(string message)
        {
            var updatedMessage = GetMessageInCorrectFormat(message);
            if (logInfo == null)
            {
                Console.WriteLine(updatedMessage);
            }
            else
            {
                logInfo(updatedMessage);
            }
        }

        /// <summary>
        /// Log an exception.
        /// </summary>
        /// <param name="exception">Exception for logging.</param>
        public void LogError(Exception exception)
        {
            if (logError == null)
            {
                Console.WriteLine(exception);
            }
            else
            {
                logError(exception);
            }
        }

        /// <summary>
        /// Log a message as warning.
        /// </summary>
        /// <param name="message">Message for logging.</param>
        public void LogWarning(string message)
        {
            var updatedMessage = GetMessageInCorrectFormat(message);
            if (logError == null)
            {
                Console.WriteLine(updatedMessage);
            }
            else
            {
                logWarning(updatedMessage);
            }
        }

        /// <summary>
        /// Log a message about waiting.
        /// </summary>
        /// <param name="formattedMessageWithoutTime">Message template for logging.</param>
        /// <param name="timeSpan">Timeout.</param>
        /// <param name="retryNumber">Number of attempt.</param>
        public void LogWaiting(string formattedMessageWithoutTime, TimeSpan timeSpan, int? retryNumber)
        {
            var waitSeconds = $"{timeSpan.TotalSeconds} seconds";
            var messageWithoutAttemptInfo = $"{string.Format(formattedMessageWithoutTime, waitSeconds)}";
            var message = messageWithoutAttemptInfo;
            if (retryNumber.HasValue)
            {
                var messageWithAttemptInfo = GetAttemptMessage(messageWithoutAttemptInfo, retryNumber.Value);
                message = messageWithAttemptInfo;
            }
            LogInfo(message);
        }

        private string GetAttemptMessage(string info, int retryNumber, string baseMessage = null)
        {
            var details = baseMessage == null ? string.Empty : $": {baseMessage}";
            return $"{info} (attempt - {retryNumber}){details}";
        }

        private string GetMessageInCorrectFormat(string message)
        {
            var updatedMessage = message.Replace('{', '\'').Replace('}', '\'');
            return $"{LoggerPrefix} {updatedMessage}";
        }
    }
}
