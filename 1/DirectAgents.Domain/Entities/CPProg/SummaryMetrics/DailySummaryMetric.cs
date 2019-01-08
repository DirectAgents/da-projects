using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg
{
    public class DailySummaryMetric : SummaryMetric
    {
        [ForeignKey("EntityId")]
        public virtual ExtAccount ExtAccount { get; set; }
    }
}