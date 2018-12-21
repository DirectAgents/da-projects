using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg
{
    public class SearchTermSummaryMetric : SummaryMetric
    {
        [ForeignKey("EntityId")]
        public virtual SearchTerm SearchTerm { get; set; }
    }
}