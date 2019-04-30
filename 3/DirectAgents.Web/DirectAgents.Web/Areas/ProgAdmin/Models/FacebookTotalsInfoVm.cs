using DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook.Models;
using System.Collections.Generic;

namespace DirectAgents.Web.Areas.ProgAdmin.Models
{
    /// <summary>
    /// Facebook Summary totals web model.
    /// </summary>
    public class FacebookTotalsInfoVm
    {
        /// <summary>
        /// Gets or sets the facebook totals information.
        /// </summary>
        /// <value>
        /// The facebook totals information.
        /// </value>
        public List<FacebookTotalsInfo> TotalsInfo { get; set; }

        /// <summary>
        /// Gets or sets the name of the level.
        /// </summary>
        /// <value>
        /// The name of the level.
        /// </value>
        public string LevelName { get; set; }
    }
}