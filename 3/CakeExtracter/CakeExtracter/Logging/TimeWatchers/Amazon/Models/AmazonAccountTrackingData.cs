using System;
using System.Collections.Generic;

namespace CakeExtracter.Logging.TimeWatchers.Amazon.Models
{
    public class AmazonAccountTrackingData
    {
        public Dictionary<string, AmazonLevelTrackingData> levelsDictionary { get; set; }

        public int AccountId { get; set; }

        public AmazonAccountTrackingData(int accountId)
        {
            AccountId = accountId;
            levelsDictionary = new Dictionary<string, AmazonLevelTrackingData>();
        }

        public void RecordOperationTime(string levelName, string operationName, TimeSpan timeSpan)
        {
            var levelTrackingData = GetLevelTrackingData(levelName);
            levelTrackingData.RecordOperationTime(operationName, timeSpan);
        }

        private AmazonLevelTrackingData GetLevelTrackingData(string levelName)
        {
            if (levelsDictionary.ContainsKey(levelName))
            {
                return levelsDictionary[levelName];
            }
            else
            {
                var levelTrackingData = new AmazonLevelTrackingData();
                levelsDictionary[levelName] = levelTrackingData;
                return levelTrackingData;
            }
        }
    }
}
