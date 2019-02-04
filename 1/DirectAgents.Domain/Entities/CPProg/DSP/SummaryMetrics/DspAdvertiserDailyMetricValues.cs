using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics
{
    /// <summary>Dsp advertiser metric values.</summary>
    public class DspAdvertiserDailyMetricValues : DspMetricValues
    {
        [ForeignKey("EntityId")]
        public virtual DspAdvertiser Advertiser { get; set; }
    }
}
