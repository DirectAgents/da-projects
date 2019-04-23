using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.YAM.Summaries
{
    public class YamDailySummary : BaseYamSummary
    {
        [ForeignKey("EntityId")]
        public virtual ExtAccount Account { get; set; }
    }
}
