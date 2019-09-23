using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook.Models
{
    /// <summary>
    /// Facebook accounts action totals.
    /// </summary>
    public class FacebookAccountActionsTotals
    {
        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public ExtAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the action totals.
        /// </summary>
        /// <value>
        /// The action totals.
        /// </value>
        public List<FacebookActionsTotals> ActionTotals { get; set; }
    }
}
