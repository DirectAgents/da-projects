using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics
{
    public class VendorProductSummaryMetric : SummaryMetric
    {
        public VendorProductSummaryMetric() : base()
        {
        }

        public VendorProductSummaryMetric(int entityId, DateTime date, MetricType metricType, decimal metricValue)
            : base(date, metricType, metricValue)
        {
            EntityId = entityId;
        }

        [ForeignKey("EntityId")]
        public virtual VendorProduct Product { get; set; }
    }
}
