using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics
{
    public class DspCreativeSummaryMetric : DspSummaryMetric
    {
        public DspCreativeSummaryMetric() : base()
        {
        }

        public DspCreativeSummaryMetric(int entityId, DateTime date, int metricTypeId, decimal metricValue)
            : base(entityId, date, metricTypeId, metricValue)
        {
        }

        [ForeignKey("EntityId")]
        public virtual DspCreative Creative { get; set; }
    }
}
