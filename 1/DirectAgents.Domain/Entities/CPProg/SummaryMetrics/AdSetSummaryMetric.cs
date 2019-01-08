using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg
{
    public class AdSetSummaryMetric : SummaryMetric
    {
        [ForeignKey("EntityId")]
        public virtual AdSet AdSet { get; set; }
    }
}