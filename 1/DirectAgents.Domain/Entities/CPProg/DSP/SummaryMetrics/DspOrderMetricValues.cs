using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics
{
    /// <summary>Dsp order metric values.</summary>
    public class DspOrderMetricValues : DspMetricValues
    {
        [ForeignKey("EntityId")]
        public virtual DspOrder Order { get; set; }
    }
}