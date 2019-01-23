﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace CakeExtractor.SeleniumApplication.Helpers
{
    internal class RetryHelper
    {
        private const int defaultAttemptCount = 3;

        public static void Do(Action action, TimeSpan retryInterval, int maxAttemptCount = defaultAttemptCount)
        {
            Do<object>(() =>
            {
                action();
                return null;
            }, retryInterval, maxAttemptCount);
        }

        public static T Do<T>(Func<T> action, TimeSpan retryInterval, int maxAttemptCount = defaultAttemptCount)
        {
            var exceptions = new List<Exception>();
            for (int attempted = 0; attempted < maxAttemptCount; attempted++)
            {
                try
                {
                    if (attempted > 0)
                    {
                        Thread.Sleep(retryInterval);
                    }
                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            throw new AggregateException(exceptions);
        }
    }
}
