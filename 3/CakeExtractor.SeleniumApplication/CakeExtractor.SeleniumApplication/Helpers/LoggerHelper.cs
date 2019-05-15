using CakeExtracter;

namespace CakeExtractor.SeleniumApplication.Helpers
{
    internal static class LoggerHelper
    {
        public static void LogWaiting(string baseMessage, int? retryCount, int accountId = 0)
        {
            var message = baseMessage;
            if (retryCount.HasValue)
            {
                message += $" (number of retrying - {retryCount})";
            }

            if (accountId > 0)
            {
                Logger.Info(message);
            }
            else
            {
                Logger.Info(accountId, message);
            }
        }
    }
}
