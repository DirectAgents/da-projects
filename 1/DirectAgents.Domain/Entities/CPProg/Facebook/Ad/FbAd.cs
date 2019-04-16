using DirectAgents.Domain.Entities.CPProg.Facebook.AdSet;
using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.Ad
{
    /// <summary>
    /// Facebook Ad database entity.
    /// </summary>
    public class FbAd : FbEntity
    {
        /// <summary>
        /// Gets or sets the status (effective status in facebook api).
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }
        
        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>
        /// The campaign identifier.
        /// </value>
        public int? CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the campaign.
        /// </summary>
        /// <value>
        /// The campaign.
        /// </value>
        [ForeignKey("CampaignId")]
        public virtual FbCampaign Campaign { get; set; }

        /// <summary>
        /// Gets or sets the ad set identifier.
        /// </summary>
        /// <value>
        /// The ad set identifier.
        /// </value>
        public int? AdSetId { get; set; }

        /// <summary>
        /// Gets or sets the ad set.
        /// </summary>
        /// <value>
        /// The ad set.
        /// </value>
        [ForeignKey("AdSetId")]
        public virtual FbAdSet AdSet { get; set; }

        /// <summary>
        /// Gets or sets the creative identifier.
        /// </summary>
        /// <value>
        /// The creative identifier.
        /// </value>
        public int? CreativeId { get; set; }

        /// <summary>
        /// Gets or sets the creative.
        /// </summary>
        /// <value>
        /// The creative.
        /// </value>
        [ForeignKey("CreativeId")]
        public virtual FbCreative Creative { get; set; }
    }
}
