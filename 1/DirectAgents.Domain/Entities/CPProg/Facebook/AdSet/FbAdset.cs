using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.AdSet
{
    /// <summary>
    /// Facebook Adset Database entity
    /// </summary>
    public class FbAdSet : FbEntity
    {
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
    }
}
