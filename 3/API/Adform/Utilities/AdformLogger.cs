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

        public AdformLogger(Action<string> logInfo, Action<Exception> logError)
        {
            this.logInfo = logInfo;
            this.logError = logError;
        }

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

        public void LogInfo(string info, int retryNumber)
        {
            var message = GetAttemptMessage(info, retryNumber);
            LogInfo(message);
        }

        public void LogGenerationError(string baseMessage, Exception exception, int retryNumber)
        {
            var info = $"{baseMessage}";
            var message = GetAttemptMessage(info, retryNumber);
            LogError(new Exception(message, exception));
        }

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
