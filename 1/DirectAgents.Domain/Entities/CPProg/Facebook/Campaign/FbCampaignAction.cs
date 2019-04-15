using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.Campaign
{
    /// <summary>
    /// Facebook campaign db entity.
    /// </summary>
    /// <seealso cref="DirectAgents.Domain.Entities.CPProg.Facebook.FbAction" />
    public class FbCampaignAction : FbAction
    {
        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>
        /// The campaign identifier.
        /// </value>
        public int CampaignId { get; set; }

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
