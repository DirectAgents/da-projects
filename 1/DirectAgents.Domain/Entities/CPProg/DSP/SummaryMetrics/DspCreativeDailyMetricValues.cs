using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics
{
    public class DspCreativeDailyMetricValues : DspMetricValues
    {
        [ForeignKey("EntityId")]
        public virtual DspCreative Creative { get; set; }
    }
}
