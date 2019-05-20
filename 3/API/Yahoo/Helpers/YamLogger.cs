using System;
using Polly;
using RestSharp;

namespace Yahoo.Helpers
{
    /// <summary>
    /// The logger of Yam utility.
    /// </summary>
    internal class YamLogger
    {
        private const string LoggerPrefix = "[YAMUtility]";

        private readonly Action<string> logInfo;
        private readonly Action<Exception> logError;
        private readonly Action<string> logWarning;

        /// <summary>
        /// Initializes a new instance of <see cref="YamLogger"/>
        /// </summary>
        /// <param name="logInfo"></param>
        /// <param name="logError"></param>
        /// <param name="logWarning"></param>
        public YamLogger(Action<string> logInfo, Action<Exception> logError, Action<string> logWarning)
        {
            this.logInfo = logInfo;
            this.logError = logError;
            this.logWarning = logWarning;
        }

        /// <summary>
        /// Log a message as info.
        /// </summary>
        /// <param name="message">Message for logging</param>
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
        /// Log a message as error.
        /// </summary>
        /// <param name="message">Message for logging</param>
        public void LogError(string message)
        {
            var exception = new Exception(message);
            LogError(exception);
        }

        /// <summary>
        /// Log an exception.
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
        /// Log a message as warning.
        /// </summary>
        /// <param name="message">Message for logging</param>
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
        /// <typeparam name="T">Type of expected results</typeparam>
        /// <param name="formattedMessageWithoutTime">Message template for logging</param>
        /// <param name="timeSpan">Timeout</param>
        /// <param name="retryNumber">Number of attempt</param>
        /// <param name="response">The captured Polly's outcome</param>
        public void LogWaiting<T>(string formattedMessageWithoutTime, TimeSpan timeSpan, int retryNumber,
            DelegateResult<IRestResponse<T>> response)
        {
            var waitDetails = response.Exception == null ? response.Result.Content : response.Exception.Message;
            var waitSeconds = $"{timeSpan.TotalSeconds} seconds";
            var message = $"{string.Format(formattedMessageWithoutTime, waitSeconds)}: {waitDetails}";
            var attemptMessage = GetAttemptMessage(message, retryNumber);
            LogInfo(attemptMessage);
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
