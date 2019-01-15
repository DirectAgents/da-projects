using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics
{
    public class VendorSubcategorySummaryMetric : SummaryMetric
    {
        public VendorSubcategorySummaryMetric() : base()
        {
        }

        public VendorSubcategorySummaryMetric(int entityId, DateTime date, MetricType metricType, decimal metricValue)
            : base(date, metricType, metricValue)
        {
            EntityId = entityId;
        }

        [ForeignKey("EntityId")]
        public virtual VendorSubcategory Subcategory { get; set; }
    }
}
