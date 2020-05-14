using System;

namespace FacebookAPI.Entities
{
    /// <summary>
    /// Facebook Campaign Reach metric row entity.
    /// </summary>
    public class FbCampaignReachRow
    {
        /// <summary>
        /// Gets or sets the Campaign identifier.
        /// </summary>
        /// <value>
        /// The campaign identifier.
        /// </value>
        public string CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the Campaign name.
        /// The name of the ad campaign you're viewing in reporting.
        /// </summary>
        public string CampaignName { get; set; }

        /// <summary>
        /// Gets or sets the start date of period for which the Reach value is calculated.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of period for which the Reach value is calculated.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the reach metric value.
        /// The number of people who saw your ads at least once.
        /// </summary>
        public int Reach { get; set; }

        /// <summary>
        /// Gets or sets the frequency metric value.
        /// The average number of times each person saw your ad.
        /// </summary>
        public double Frequency { get; set; }
    }
}