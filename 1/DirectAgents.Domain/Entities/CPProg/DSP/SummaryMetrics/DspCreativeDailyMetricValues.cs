using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics
{
    /// <summary>Dsp creative metric values.</summary>
    public class DspCreativeDailyMetricValues : DspMetricValues
    {
        [ForeignKey("EntityId")]
        public virtual DspCreative Creative { get; set; }
    }
}
