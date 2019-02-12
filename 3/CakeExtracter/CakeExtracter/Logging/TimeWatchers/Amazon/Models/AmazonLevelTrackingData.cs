using System;
using System.Collections.Generic;

namespace CakeExtracter.Logging.TimeWatchers.Amazon.Models
{
    public class AmazonLevelTrackingData
    {
        public Dictionary<string, TimeSpan> operationsTimeInfo;

        public AmazonLevelTrackingData()
        {
            operationsTimeInfo = new Dictionary<string, TimeSpan>();
        }

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
