using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.AdSet
{
    /// <summary>
    /// Facebook Adset Summary entity
    /// </summary>
    /// <seealso cref="DirectAgents.Domain.Entities.CPProg.Facebook.FbBaseSummary" />
    public class FbAdSetSummary : FbBaseSummary
    {
        /// <summary>
        /// Gets or sets the AdSet identifier.
        /// </summary>
        /// <value>
        /// The AdSet identifier.
        /// </value>
        public int AdSetId { get; set; }

        /// <summary>
        /// Gets or sets the AdSet.
        /// </summary>
        /// <value>
        /// The ad set.
        /// </value>
        [ForeignKey("AdSetId")]
        public virtual FbAdSet AdSet { get; set; }

        /// <summary>
        /// Gets or sets the actions of related AdSet.
        /// </summary>
        /// <value>
        /// The actions.
        /// </value>
        [NotMapped]
        public List<FbAdSetAction> Actions { get; set; }
    }
}
