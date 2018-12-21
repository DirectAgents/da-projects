using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg
{
    public class TDadSummaryMetric : SummaryMetric
    {
        [ForeignKey("EntityId")]
        public virtual TDad TDad { get; set; }
    }
}