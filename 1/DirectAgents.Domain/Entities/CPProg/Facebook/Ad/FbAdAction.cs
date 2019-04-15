using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.Ad
{
    /// <summary>
    /// Facebook Ad action database entity
    /// </summary>
    /// <seealso cref="DirectAgents.Domain.Entities.CPProg.Facebook.FbAction" />
    public class FbAdAction : FbAction
    {
        /// <summary>
        /// Gets or sets the ad identifier.
        /// </summary>
        /// <value>
        /// The ad identifier.
        /// </value>
        public int AdId { get; set; }

        /// <summary>
        /// Gets or sets the ad.
        /// </summary>
        /// <value>
        /// The ad.
        /// </value>
        [ForeignKey("AdId")]
        public virtual FbAd Ad { get; set; }
    }
}
