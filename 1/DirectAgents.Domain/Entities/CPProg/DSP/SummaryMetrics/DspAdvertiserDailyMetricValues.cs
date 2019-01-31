using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics
{
    public class DspAdvertiserDailyMetricValues : DspMetricValues
    {
        [ForeignKey("EntityId")]
        public virtual DspAdvertiser Advertiser { get; set; }
    }
}
