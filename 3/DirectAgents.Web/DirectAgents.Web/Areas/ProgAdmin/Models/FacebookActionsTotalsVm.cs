using System.Collections.Generic;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook.Models;

namespace DirectAgents.Web.Areas.ProgAdmin.Models
{
    /// <summary>
    /// View model for account action totals.
    /// </summary>
    public class FacebookActionsTotalsVm
    {
        /// <summary>
        /// Gets or sets the account actions totals information.
        /// </summary>
        /// <value>
        /// The account actions totals information.
        /// </value>
        public List<FacebookAccountActionsTotals> AccountActionsTotals { get; set; }

        /// <summary>
        /// Gets or sets the name of the level.
        /// </summary>
        /// <value>
        /// The name of the level.
        /// </value>
        public string LevelName { get; set; }
    }
}