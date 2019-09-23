using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.AdSet
{
    /// <summary>
    /// Facebook Adset Action
    /// </summary>
    /// <seealso cref="DirectAgents.Domain.Entities.CPProg.Facebook.FbAction" />
    public class FbAdSetAction : FbAction
    {
        /// <summary>
        /// Gets or sets the ad set identifier.
        /// </summary>
        /// <value>
        /// The ad set identifier.
        /// </value>
        public int AdSetId { get; set; }

        /// <summary>
        /// Gets or sets the ad set.
        /// </summary>
        /// <value>
        /// The ad set.
        /// </value>
        [ForeignKey("AdSetId")]
        public virtual FbAdSet AdSet { get; set; }
    }
}
