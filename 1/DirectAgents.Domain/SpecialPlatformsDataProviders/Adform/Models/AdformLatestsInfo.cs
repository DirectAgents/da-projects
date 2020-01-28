using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.Adform.Models
{
    public class AdformLatestsInfo
    {
        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public ExtAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the lineItem latests information.
        /// </summary>
        /// <value>
        /// The LineItem latests information.
        /// </value>
        public SpecialPlatformLatestsSummary LineItemLatestsInfo { get; set; }

        /// <summary>
        /// Gets or sets the Banner latests information.
        /// </summary>
        /// <value>
        /// The Banner latests information.
        /// </value>
        public SpecialPlatformLatestsSummary BannerLatestsInfo { get; set; }

        /// <summary>
        /// Gets or sets the Campaign latests information.
        /// </summary>
        /// <value>
        /// The Campaign latests information.
        /// </value>
        public SpecialPlatformLatestsSummary CampaignLatestsInfo { get; set; }

        /// <summary>
        /// Gets or sets the Daily latests information.
        /// </summary>
        /// <value>
        /// The Daily latests information.
        /// </value>
        public SpecialPlatformLatestsSummary DailyLatestsInfo { get; set; }
    }
}
