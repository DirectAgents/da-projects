using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg
{
    public class KeywordSummaryMetric : SummaryMetric
    {
        [ForeignKey("EntityId")]
        public virtual Keyword Keyword { get; set; }
    }
}