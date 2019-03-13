using System;
using Polly;
using RestSharp;

namespace CommissionJunction.Utilities
{
    internal class CjLogger
    {
        private const string LoggerPrefix = "[Commission Junction]";

        private readonly Action<string> logInfo;
        private readonly Action<string> logError;

        public CjLogger(Action<string> logInfo, Action<string> logError)
        {
            this.logInfo = logInfo;
            this.logError = logError;
        }

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

        public void LogError(string message)
        {
            var updatedMessage = GetMessageInCorrectFormat(message);
            if (logError == null)
            {
                Console.WriteLine(updatedMessage);
            }
            else
            {
                logError(updatedMessage);
            }
        }

        public void LogWaiting<T>(string formattedMessageWithoutTime, TimeSpan timeSpan, int retryNumber, DelegateResult<IRestResponse<T>> response)
        {
            var waitDetails = response.Exception == null ? response.Result.Content : response.Exception.Message;
            LogWaiting(formattedMessageWithoutTime, timeSpan, retryNumber, waitDetails);
        }

        private void LogInfo(string info, int retryNumber)
        {
            var message = GetAttemptMessage(info, retryNumber);
            LogInfo(message);
        }

        private void LogWaiting(string formattedMessageWithoutTime, TimeSpan timeSpan, int retryNumber, string waitDetails)
        {
            var waitSeconds = timeSpan.TotalSeconds;
            var message = $"{string.Format(formattedMessageWithoutTime, waitSeconds)}: {waitDetails}";
            LogInfo(message, retryNumber);
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
