using System;

namespace Adform.Utilities
{
    /// <summary>
    /// The logger of Adform utility
    /// </summary>
    internal class AdformLogger
    {
        private const string LoggerPrefix = "[AdformUtility]";

        private readonly Action<string> logInfo;
        private readonly Action<Exception> logError;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdformLogger"/> class.
        /// </summary>
        /// <param name="logInfo">Action that logs infos</param>
        /// <param name="logError">Action that logs errors</param>
        public AdformLogger(Action<string> logInfo, Action<Exception> logError)
        {
            this.logInfo = logInfo;
            this.logError = logError;
        }

        /// <summary>
        /// Log an exception
        /// </summary>
        /// <param name="exception">Exception for logging</param>
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
        /// Log a message with information about retry attempts
        /// </summary>
        /// <param name="info">Message for logging</param>
        /// <param name="retryNumber">Number of attempt</param>
        public void LogInfo(string info, int retryNumber)
        {
            var message = GetAttemptMessage(info, retryNumber);
            LogInfo(message);
        }

        /// <summary>
        /// Log an exception with additional information and retry attempts
        /// </summary>
        /// <param name="baseMessage">Message for logging</param>
        /// <param name="exception">Exception for logging</param>
        /// <param name="retryNumber">Number of attempt</param>
        public void LogGenerationError(string baseMessage, Exception exception, int retryNumber)
        {
            var info = $"{baseMessage}";
            var message = GetAttemptMessage(info, retryNumber);
            LogError(new Exception(message, exception));
        }

        /// <summary>
        /// Log a message about waiting
        /// </summary>
        /// <param name="baseMessage">Message for logging</param>
        /// <param name="retryCount">Number of attempt</param>
        public void LogWaiting(string baseMessage, int? retryCount)
        {
            if (retryCount.HasValue)
            {
                baseMessage += $" (number of retrying - {retryCount})";
            }
            LogInfo(baseMessage);
        }

        private void LogInfo(string message)
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

        private static string GetMessageInCorrectFormat(string message)
        {
            var updatedMessage = message.Replace('{', '\'').Replace('}', '\'');
            return $"{LoggerPrefix} {updatedMessage}";
        }

        private static string GetAttemptMessage(string info, int retryNumber, string baseMessage = null)
        {
            var details = baseMessage == null ? string.Empty : $": {baseMessage}";
            return $"{info} (attempt - {retryNumber}){details}";
        }
    }
}
