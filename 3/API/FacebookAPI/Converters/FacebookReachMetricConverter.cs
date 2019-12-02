using System;
using FacebookAPI.Entities;

namespace FacebookAPI.Converters
{
    /// <summary>
    /// Facebook Reach metric converter.
    /// </summary>
    internal class FacebookReachMetricConverter : IFacebookConverter<FbReachRow>
    {
        /// <summary>
        /// Gets the Facebook Reach metric from row.
        /// </summary>
        /// <param name="row">Report row.</param>
        /// <returns>Facebook Reach metric.</returns>
        public FbReachRow ParseSummaryFromRow(dynamic row)
        {
            return new FbReachRow
            {
                StartDate = DateTime.Parse(row.date_start),
                EndDate = DateTime.Parse(row.date_stop),
                Reach = int.Parse(row.reach),
            };
        }
    }
}
