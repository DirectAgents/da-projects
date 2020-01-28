using System.Collections.Generic;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Adform.Models;

namespace DirectAgents.Web.Areas.ProgAdmin.Models.Adform
{
    /// <summary>
    /// Web model for latests Adform info.
    /// </summary>
    public class AdformLatestsInfoVm
    {
        /// <summary>
        /// Gets or sets the latests information.
        /// </summary>
        /// <value>
        /// The latests information.
        /// </value>
        public List<AdformLatestsInfo> LatestsInfo { get; set; }
    }
}