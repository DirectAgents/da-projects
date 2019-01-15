using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics
{
    public class VendorCategorySummaryMetric : SummaryMetric
    {
        public VendorCategorySummaryMetric() : base()
        {
        }

        public VendorCategorySummaryMetric(int entityId, DateTime date, MetricType metricType, decimal metricValue) 
            : base(date, metricType, metricValue)
        {
            EntityId = entityId;
        }

        [ForeignKey("EntityId")]
        public virtual VendorCategory Category { get; set; }
    }
}
