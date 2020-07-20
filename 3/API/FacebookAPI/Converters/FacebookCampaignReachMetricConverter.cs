using System;

using FacebookAPI.Entities;

namespace FacebookAPI.Converters
{
    /// <summary>
    /// Facebook Campaign Reach metric converter.
    /// </summary>
    internal class FacebookCampaignReachMetricConverter : IFacebookConverter<FbCampaignReachRow>
    {
        private readonly Action<string> logWarn;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookCampaignReachMetricConverter"/> class.
        /// </summary>
        /// <param name="logWarn">Action for logging (the warn level).</param>
        public FacebookCampaignReachMetricConverter(Action<string> logWarn)
        {
            this.logWarn = logWarn;
        }

        /// <summary>
        /// Gets the Facebook Campaign Reach metric from row.
        /// </summary>
        /// <param name="row">Report row.</param>
        /// <returns>Facebook Reach metric.</returns>
        public FbCampaignReachRow ParseSummaryFromRow(dynamic row)
        {
            CheckRow(row);

            return new FbCampaignReachRow
            {
                CampaignId = row.campaign_id,
                CampaignName = row.campaign_name,
                StartDate = DateTime.Parse(row.date_start),
                EndDate = DateTime.Parse(row.date_stop),
                Reach = Convert.ToInt32(row.reach),
                Frequency = Convert.ToDouble(row.frequency),
            };
        }

        private void CheckRow(dynamic row)
        {
            const string LogMessage = "{0} metric is null";

            if (IsNullMetric(row.reach))
            {
                LogWarn(LogMessage, "Reach");
            }

            if (IsNullMetric(row.frequency))
            {
                LogWarn(LogMessage, "Frequency");
            }
        }

        private bool IsNullMetric(dynamic metric)
        {
            return object.ReferenceEquals(null, metric);
        }

        private void LogWarn(string message, params object[] args)
        {
            logWarn(string.Format(message, args));
        }
    }
}