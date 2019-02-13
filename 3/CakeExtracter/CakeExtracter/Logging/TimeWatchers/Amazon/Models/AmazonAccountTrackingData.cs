using System;
using System.Collections.Generic;

namespace CakeExtracter.Logging.TimeWatchers.Amazon.Models
{
    /// <summary>
    /// Amazon Account Tracking Execution Data 
    /// </summary>
    public class AmazonAccountTrackingData
    {
        /// <summary>
        /// Gets the levels time tracking dictionary.
        /// </summary>
        /// <value>
        /// The levels dictionary.
        /// </value>
        public Dictionary<string, AmazonLevelTrackingData> levelsDictionary { get; }

        public int AccountId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonAccountTrackingData"/> class.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        public AmazonAccountTrackingData(int accountId)
        {
            AccountId = accountId;
            levelsDictionary = new Dictionary<string, AmazonLevelTrackingData>();
        }

        /// <summary>
        /// Records the operation execution time.
        /// </summary>
        /// <param name="levelName">Name of the level.</param>
        /// <param name="operationName">Name of the operation.</param>
        /// <param name="timeSpan">The time span.</param>
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
