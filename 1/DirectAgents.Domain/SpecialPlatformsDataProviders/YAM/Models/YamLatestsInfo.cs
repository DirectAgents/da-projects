using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.YAM.Models
{
    /// <summary>
    /// Latests info for YAM accounts(earliest date, latest date).
    /// </summary>
    public class YamLatestsInfo
    {
        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public ExtAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the Daily latests information.
        /// </summary>
        /// <value>
        /// The Daily latests information.
        /// </value>
        public SpecialPlatformLatestsSummary DailyLatestsInfo { get; set; }

        /// <summary>
        /// Gets or sets the Campaign latests information.
        /// </summary>
        /// <value>
        /// The Campaign latests information.
        /// </value>
        public SpecialPlatformLatestsSummary CampaignLatestsInfo { get; set; }

        /// <summary>
        /// Gets or sets the Ad latests information.
        /// </summary>
        /// <value>
        /// The Ad latests information.
        /// </value>
        public SpecialPlatformLatestsSummary AdLatestsInfo { get; set; }

        /// <summary>
        /// Gets or sets the Creative latests information.
        /// </summary>
        /// <value>
        /// The Creative latests information.
        /// </value>
        public SpecialPlatformLatestsSummary CreativeLatestsInfo { get; set; }

        /// <summary>
        /// Gets or sets the Line latests information.
        /// </summary>
        /// <value>
        /// The Line latests information.
        /// </value>
        public SpecialPlatformLatestsSummary LineLatestsInfo { get; set; }

        /// <summary>
        /// Gets or sets the Pixel latests information.
        /// </summary>
        /// <value>
        /// The Pixel latests information.
        /// </value>
        public SpecialPlatformLatestsSummary PixelLatestsInfo { get; set; }
    }
}
