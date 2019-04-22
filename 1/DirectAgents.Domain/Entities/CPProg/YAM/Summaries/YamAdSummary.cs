using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.YAM.Summaries
{
    public class YamAdSummary : BaseYamSummary
    {
        [ForeignKey("EntityId")]
        public virtual YamAd Ad { get; set; }
    }
}
