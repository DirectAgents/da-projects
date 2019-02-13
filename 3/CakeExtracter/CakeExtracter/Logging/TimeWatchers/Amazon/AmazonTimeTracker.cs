using CakeExtracter.Logging.TimeWatchers.Amazon.Models;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using System;
using System.Collections.Generic;

namespace CakeExtracter.Logging.TimeWatchers.Amazon
{
    /// <summary>
    /// Time Tracker Singleton object for performance measurements.
    /// </summary>
    public class AmazonTimeTracker
    {
        private Dictionary<int, AmazonAccountTrackingData> accountsTrackingData;

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>
        /// The instance of time tracker.
        /// </value>
        public static AmazonTimeTracker Instance
        {
            get;
        }

        private AmazonTimeTracker()
        {
            accountsTrackingData = new Dictionary<int, AmazonAccountTrackingData>();
        }

        /// <summary>
        /// Initializes the instance of singleton object.
        /// </summary>
        static AmazonTimeTracker()
        {
            Instance = new AmazonTimeTracker();
        }

        /// <summary>
        /// Executes specified action with time tracking. Record action time.
        /// </summary>
        /// <param name="watchableAction">The watchable action.</param>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="levelName">Name of the level.</param>
        /// <param name="operationName">Name of the operation.</param>
        public void ExecuteWithTimeTracking(Action watchableAction, int accountId, string levelName, string operationName)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            watchableAction();
            watch.Stop();
            RecordLevelOperationTime(accountId, levelName, operationName, watch.Elapsed);
        }

        /// <summary>
        /// Logs the tracking data.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        public void LogTrackingData(int accountId)
        {
            var accountTrackingData = GetAccountTrackingData(accountId);
            Logger.Info(accountId, "Time tracking data for {0} account.", accountId);
            accountTrackingData.levelsDictionary.ForEach(kvPair =>
            {
                Logger.Info(accountId, "Level: {0}", kvPair.Key);
                var levelOperationDictionary = kvPair.Value.operationsTimeInfo;
                levelOperationDictionary.ForEach(operKVPair =>
                {
                    Logger.Info(accountId, "Operation: {0} - Time {1}", operKVPair.Key, operKVPair.Value);
                });
            });
        }

        private void RecordLevelOperationTime(int accountId, string levelName, string operationName, TimeSpan time)
        {
            var accountTrackingData = GetAccountTrackingData(accountId);
            accountTrackingData.RecordOperationTime(levelName, operationName, time);
        }

        private AmazonAccountTrackingData GetAccountTrackingData(int accountId)
        {
            if (accountsTrackingData.ContainsKey(accountId))
            {
                return accountsTrackingData[accountId];
            }
            else
            {
                var accountTrackingData = new AmazonAccountTrackingData(accountId);
                accountsTrackingData.Add(accountId, accountTrackingData);
                return accountTrackingData;
            }
        }
    }
}
