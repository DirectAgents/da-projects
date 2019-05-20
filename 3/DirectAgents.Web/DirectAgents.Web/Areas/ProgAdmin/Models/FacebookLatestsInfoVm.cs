using DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook.Models;
using System.Collections.Generic;

namespace DirectAgents.Web.Areas.ProgAdmin.Models
{
    /// <summary>
    /// Web model for latests facebook info
    /// </summary>
    public class FacebookLatestsInfoVm
    {
        /// <summary>
        /// Gets or sets the latests information.
        /// </summary>
        /// <value>
        /// The latests information.
        /// </value>
        public List<FacebookLatestsInfo> LatestsInfo { get; set; }
    }
}