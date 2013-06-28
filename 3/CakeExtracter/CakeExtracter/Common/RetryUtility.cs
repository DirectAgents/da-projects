using System;
using System.Linq;

namespace CakeExtracter.Common
{
    public static class RetryUtility
    {
        public static T Retry<T>(
            Func<T> action,
            int maxAttempts,
            int pauseMillisecondsBetweenAttempts,
            params Type[] retryWhenCaught)
        {
            return Retry(action, maxAttempts, pauseMillisecondsBetweenAttempts, 0, retryWhenCaught);
        }

        private static T Retry<T>(
            Func<T> action,
            int maxAttempts,
            int pauseMillisecondsBetweenAttempts,
            int attemptNumber,
            params Type[] retryWhenCaught)
        {
            try
            {
                Logger.Info("Attempt #{0}..", attemptNumber);
                return action();
            }
            catch (Exception ex)
            {
                if (retryWhenCaught.Any(c => c == ex.GetType()))
                {
                    Logger.Warn("Caught {0}: {1}", ex.GetType().Name, ex.Message);
                    if (attemptNumber < maxAttempts)
                    {
                        Logger.Info("Pausing for {0} ms before next attempt..", pauseMillisecondsBetweenAttempts);
                        Retry(action, maxAttempts, pauseMillisecondsBetweenAttempts, attemptNumber + 1, retryWhenCaught);
                    }
                }
                throw;
            }
        }
    }
}