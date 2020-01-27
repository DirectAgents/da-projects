using System.Collections.Generic;
using DirectAgents.Domain.SpecialPlatformsDataProviders.DBM.Models;

namespace DirectAgents.Web.Areas.ProgAdmin.Models.DBM
{
    /// <summary>
    /// Web model for latests DBM info.
    /// </summary>
    public class DbmLatestsInfoVm
    {
        /// <summary>
        /// Gets or sets the latests information.
        /// </summary>
        /// <value>
        /// The latests information.
        /// </value>
        public List<DbmLatestsInfo> LatestsInfo { get; set; }
    }
}