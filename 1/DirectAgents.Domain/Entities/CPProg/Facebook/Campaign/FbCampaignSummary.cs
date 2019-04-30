using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.Campaign
{
    /// <summary>
    /// Facebook Adset Summary Entity
    /// </summary>
    /// <seealso cref="DirectAgents.Domain.Entities.CPProg.Facebook.FbBaseSummary" />
    public class FbCampaignSummary : FbBaseSummary
    {
        /// <summary>
        /// Gets or sets the Campaign identifier.
        /// </summary>
        /// <value>
        /// The campaign identifier.
        /// </value>
        public int CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the related Campaign entity.
        /// </summary>
        /// <value>
        /// The campaign.
        /// </value>
        [ForeignKey("CampaignId")]
        public virtual FbCampaign Campaign { get; set; }
    }
}
