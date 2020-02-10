using System;
using FacebookAPI.Entities;

namespace FacebookAPI.Converters
{
    /// <summary>
    /// Facebook Reach metric converter.
    /// </summary>
    internal class FacebookReachMetricConverter : IFacebookConverter<FbReachRow>
    {
        private Action<string> _LogWarn;

        private void LogWarn(string message)
        {
            _LogWarn(message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookReachMetricConverter"/> class.
        /// </summary>
        /// <param name="logWarn">Action for logging (the warn level).</param>
        public FacebookReachMetricConverter( Action<string> logWarn)
        {
            _LogWarn = logWarn;
        }

        /// <summary>
        /// Gets the Facebook Reach metric from row.
        /// </summary>
        /// <param name="row">Report row.</param>
        /// <returns>Facebook Reach metric.</returns>
        public FbReachRow ParseSummaryFromRow(dynamic row)
        {
            var richMetric = row.reach;

            if (string.IsNullOrEmpty(row.reach))
            {
                const string defaultReachMetricString = "0";
                richMetric = defaultReachMetricString;
                LogWarn("Reach metric is null");
            }

            return new FbReachRow
            {
                StartDate = DateTime.Parse(row.date_start),
                EndDate = DateTime.Parse(row.date_stop),
                Reach = int.Parse(richMetric),
            };
        }
    }
}
