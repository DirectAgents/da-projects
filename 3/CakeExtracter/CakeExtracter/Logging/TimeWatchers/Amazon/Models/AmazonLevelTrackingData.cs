using System;
using System.Collections.Generic;

namespace CakeExtracter.Logging.TimeWatchers.Amazon.Models
{
    /// <summary>
    /// Amazon level tracing execution data.
    /// </summary>
    public class AmazonLevelTrackingData
    {
        /// <summary>
        /// The operations time performance information
        /// </summary>
        public Dictionary<string, TimeSpan> operationsTimeInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonLevelTrackingData"/> class.
        /// </summary>
        public AmazonLevelTrackingData()
        {
            operationsTimeInfo = new Dictionary<string, TimeSpan>();
        }

        /// <summary>
        /// Records the operation execution time.
        /// </summary>
        /// <param name="operationName">Name of the operation.</param>
        /// <param name="timeSpan">The time span.</param>
        public void RecordOperationTime(string operationName, TimeSpan timeSpan)
        {
            var currentTimeSpan = GetOperationTimeSpan(operationName);
            currentTimeSpan = currentTimeSpan.Add(timeSpan);
            operationsTimeInfo[operationName] = currentTimeSpan;
        }

        private TimeSpan GetOperationTimeSpan(string operationName)
        {
            if (operationsTimeInfo.ContainsKey(operationName))
            {
                return operationsTimeInfo[operationName];
            }
            else
            {
                var operationTimeSpan = new TimeSpan();
                operationsTimeInfo[operationName] = operationTimeSpan;
                return operationTimeSpan;
            }
        }
    }
}
