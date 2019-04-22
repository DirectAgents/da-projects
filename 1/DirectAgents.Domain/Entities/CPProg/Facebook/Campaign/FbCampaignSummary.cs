using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.Campaign
{
    /// <summary>
    /// Facebook Adset Summary Entity
    /// </summary>
    /// <seealso cref="DirectAgents.Domain.Entities.CPProg.Facebook.FbBaseSummary" />
    public class FbCampaignSummary : FbBaseSummary
    {
        public int CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public virtual FbCampaign Campaign { get; set; }
    }
}
