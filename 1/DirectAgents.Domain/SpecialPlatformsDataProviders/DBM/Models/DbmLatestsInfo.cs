using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.DBM.Models
{
    public class DbmLatestsInfo
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
        /// Gets or sets the creative latests information.
        /// </summary>
        /// <value>
        /// The Creative latests information.
        /// </value>
        public SpecialPlatformLatestsSummary CreativeLatestsInfo { get; set; }
    }
}
