using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics
{
    /// <summary>Dsp line items daily metric values.</summary>
    public class DspLineDailyMetricValues : DspMetricValues
    {
        [ForeignKey("EntityId")]
        public virtual DspLineItem LineItem { get; set; }
    }
}
