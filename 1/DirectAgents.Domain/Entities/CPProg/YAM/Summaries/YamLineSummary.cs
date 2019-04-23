using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.YAM.Summaries
{
    public class YamLineSummary : BaseYamSummary
    {
        [ForeignKey("EntityId")]
        public virtual YamLine Line { get; set; }
    }
}
