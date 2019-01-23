using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics
{
    public class VendorProductSummaryMetric : SummaryMetric
    {
        public VendorProductSummaryMetric() : base()
        {
        }

        public VendorProductSummaryMetric(int entityId, DateTime date, int metricTypeId, decimal metricValue)
        {
            EntityId = entityId;
            MetricTypeId = metricTypeId;
            Date = date;
            Value = metricValue;
        }

        [ForeignKey("EntityId")]
        public virtual VendorProduct Product { get; set; }
    }
}
