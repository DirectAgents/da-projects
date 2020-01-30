using System.Collections.Generic;
using DirectAgents.Domain.SpecialPlatformsDataProviders.YAM.Models;

namespace DirectAgents.Web.Areas.ProgAdmin.Models.YAM
{
    /// <summary>
    /// Web model for latests YAM info.
    /// </summary>
    public class YamLatestsInfoVm
    {
        /// <summary>
        /// Gets or sets the latests information.
        /// </summary>
        /// <value>
        /// The latests information.
        /// </value>
        public List<YamLatestsInfo> LatestsInfo { get; set; }
    }
}