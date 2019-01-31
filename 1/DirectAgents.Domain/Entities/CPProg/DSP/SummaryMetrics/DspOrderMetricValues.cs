using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics
{
    public class DspOrderMetricValues : DspMetricValues
    {
        [ForeignKey("EntityId")]
        public virtual DspOrder Order { get; set; }
    }
}