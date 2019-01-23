using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics
{
    public class VendorSubcategorySummaryMetric : SummaryMetric
    {
        public VendorSubcategorySummaryMetric() : base()
        {
        }

        public VendorSubcategorySummaryMetric(int entityId, DateTime date, int metricTypeId, decimal metricValue)
        {
            EntityId = entityId;
            MetricTypeId = metricTypeId;
            Date = date;
            Value = metricValue;
        }

        [ForeignKey("EntityId")]
        public virtual VendorSubcategory Subcategory { get; set; }
    }
}
