using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Adform.Summaries
{
    public class AdfBannerSummary : AdfBaseSummary
    {
        [ForeignKey("EntityId")]
        public virtual AdfBanner Banner { get; set; }
    }
}
