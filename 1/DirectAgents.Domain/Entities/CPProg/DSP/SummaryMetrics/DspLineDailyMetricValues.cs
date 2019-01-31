using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics
{
    public class DspLineDailyMetricValues : DspMetricValues
    {
        [ForeignKey("EntityId")]
        public virtual DspLineItem LineItem { get; set; }
    }
}
