using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.Ad
{
    /// <summary>
    /// Facebook Ad Summary entity
    /// </summary>
    public class FbAdSummary : FbBaseSummary
    {
        /// <summary>
        /// Gets or sets the related Ad's identifier.
        /// </summary>
        /// <value>
        /// The ad identifier.
        /// </value>
        public int AdId { get; set; }

        /// <summary>
        /// Gets or sets the related Ad entity.
        /// </summary>
        /// <value>
        /// The ad.
        /// </value>
        [ForeignKey("AdId")]
        public virtual FbAd Ad { get; set; }

        /// <summary>
        /// Gets or sets the corresponding Ad's actions.
        /// </summary>
        /// <value>
        /// The actions.
        /// </value>
        [NotMapped]
        public List<FbAdAction> Actions { get; set; }
    }
}
