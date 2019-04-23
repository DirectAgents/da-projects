using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.YAM.Summaries
{
    public class YamCampaignSummary : BaseYamSummary
    {
        [ForeignKey("EntityId")]
        public virtual YamCampaign Campaign { get; set; }
    }
}
