using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.YAM.Summaries
{
    public class YamCreativeSummary : BaseYamSummary
    {
        [ForeignKey("EntityId")]
        public virtual YamCreative Creative { get; set; }
    }
}
