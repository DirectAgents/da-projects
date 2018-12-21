using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg
{
    public class StrategySummaryMetric : SummaryMetric
    {
        [ForeignKey("EntityId")]
        public virtual Strategy Strategy { get; set; }
    }
}