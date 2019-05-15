using System;

namespace CakeExtractor.SeleniumApplication.Helpers
{
    internal static class LoggerHelper
    {
        public static void LogWaiting(string baseMessage, int? retryCount, Action<string> log)
        {
            var message = baseMessage;
            if (retryCount.HasValue)
            {
                message += $" (number of retrying - {retryCount})";
            }
            log(message);
        }
    }
}
