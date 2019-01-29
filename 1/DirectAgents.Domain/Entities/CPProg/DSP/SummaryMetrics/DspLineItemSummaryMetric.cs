using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics
{
    public class DspLineItemSummaryMetric : DspSummaryMetric
    {
        public DspLineItemSummaryMetric() : base()
        {
        }

        public DspLineItemSummaryMetric(int entityId, DateTime date, int metricTypeId, decimal metricValue)
            : base(entityId, date, metricTypeId, metricValue)
        {
        }

        [ForeignKey("EntityId")]
        public virtual DspLineItem LineItem { get; set; }
    }
}
