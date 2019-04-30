using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook.Models
{
    /// <summary>
    /// Latests info for Facebook accounts(earliest date, latest date)
    /// </summary>
    public class FacebookLatestsInfo
    {
        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public ExtAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the daily latests information.
        /// </summary>
        /// <value>
        /// The daily latests information.
        /// </value>
        public SpecialPlatformLatestsSummary DailyLatestsInfo { get; set; }

        /// <summary>
        /// Gets or sets the campaign latests information.
        /// </summary>
        /// <value>
        /// The campaign latests information.
        /// </value>
        public SpecialPlatformLatestsSummary CampaignLatestsInfo { get; set; }

        /// <summary>
        /// Gets or sets the Adsets latests information.
        /// </summary>
        /// <value>
        /// The Adsets latests information.
        /// </value>
        public SpecialPlatformLatestsSummary AdsetsLatestsInfo { get; set; }

        /// <summary>
        /// Gets or sets the Adsets actions latests information.
        /// </summary>
        /// <value>
        /// The Adsets actions latests information.
        /// </value>
        public SpecialPlatformLatestsSummary AdsetsActionsLatestsInfo { get; set; }

        /// <summary>
        /// Gets or sets the aAs latests information.
        /// </summary>
        /// <value>
        /// The ads latest information.
        /// </value>
        public SpecialPlatformLatestsSummary AdsLatestsInfo { get; set; }

        /// <summary>
        /// Gets or sets the Ads actions latests information.
        /// </summary>
        /// <value>
        /// The ads actions latests information.
        /// </value>
        public SpecialPlatformLatestsSummary AdsActionsLatestsInfo { get; set; }
    }
}
