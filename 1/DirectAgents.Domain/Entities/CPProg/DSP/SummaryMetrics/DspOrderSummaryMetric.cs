using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics
{
    public class DspOrderSummaryMetric : DspSummaryMetric
    {
        public DspOrderSummaryMetric() : base()
        {
        }

        public DspOrderSummaryMetric(int entityId, DateTime date, int metricTypeId, decimal metricValue)
            : base(entityId, date, metricTypeId, metricValue)
        {
        }

        [ForeignKey("EntityId")]
        public virtual DspOrder Order { get; set; }
    }
}
