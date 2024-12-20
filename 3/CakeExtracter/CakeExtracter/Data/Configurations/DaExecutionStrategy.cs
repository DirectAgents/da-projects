﻿using System;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;

namespace CakeExtracter.Data.Configurations
{
    /// <summary>
    /// Execution strategy to retry db request for timeout and deadlock errors.
    /// </summary>
    public class DaExecutionStrategy : SqlAzureExecutionStrategy
    {
        private const int SqlErrorDeadlockNumber = 1205;

        private const int SqlErrorTimeoutNumber = -2;

        private const int SqlErrorConnectionClosed = 19;

        /// <summary>
        /// Initializes a new instance of the <see cref="DaExecutionStrategy"/> class.
        /// </summary>
        public DaExecutionStrategy()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DaExecutionStrategy"/> class.
        /// </summary>
        /// <param name="maxRetryCount">Max retry attempts count.</param>
        /// <param name="maxDelay">Max delay between retry attempts.</param>
        public DaExecutionStrategy(int maxRetryCount, TimeSpan maxDelay)
            : base(maxRetryCount, maxDelay)
        {
        }

        /// <inheritdoc/>
        protected override bool ShouldRetryOn(Exception ex)
        {
            var isRetryRequest = false;

            if (ex is SqlException sqlException)
            {
                int[] errorsToRetry = { SqlErrorDeadlockNumber, SqlErrorTimeoutNumber, SqlErrorConnectionClosed };
                if (sqlException.Errors.Cast<SqlError>().Any(x => errorsToRetry.Contains(x.Number)))
                {
                    Logger.Warn($"[DaExecutionStrategy]: Retry database request for {ex.Message}");
                    isRetryRequest = true;
                }
                else if (base.ShouldRetryOn(ex))
                {
                    Logger.Warn($"[DaExecutionStrategy]: Retry database request by base Azure Execution strategy for {ex.Message}");
                    isRetryRequest = true;
                }
                else
                {
                    Logger.Warn($"[DaExecutionStrategy]: {ex.Message} doesn't support db request retry strategy.");
                }
            }

            if (ex is TimeoutException)
            {
                Logger.Warn($"[DaExecutionStrategy]: Retry database request for {ex.Message}");
                isRetryRequest = true;
            }

            return isRetryRequest;
        }
    }
}