using CakeExtracter.Logging.TimeWatchers.Amazon.Models;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using System;
using System.Collections.Generic;

namespace CakeExtracter.Logging.TimeWatchers.Amazon
{
    public class AmazonTimeTracker
    {
        private Dictionary<int, AmazonAccountTrackingData> accountsTrackingData;

        public static AmazonTimeTracker Instance
        {
            get;
        }

        private AmazonTimeTracker()
        {
            accountsTrackingData = new Dictionary<int, AmazonAccountTrackingData>();
        }

        static AmazonTimeTracker()
        {
            Instance = new AmazonTimeTracker();
        }

        public void RecordLevelOperationTime(int accountId, string levelName, string operationName, TimeSpan time)
        {
            var accountTrackingData = GetAccountTrackingData(accountId);
            accountTrackingData.RecordOperationTime(levelName, operationName, time);
        }

        public void LogTrackingData(int accountId)
        {
            var accountTrackingData = GetAccountTrackingData(accountId);
            Logger.Info(accountId, "Time tracking data for {0} account.");
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
