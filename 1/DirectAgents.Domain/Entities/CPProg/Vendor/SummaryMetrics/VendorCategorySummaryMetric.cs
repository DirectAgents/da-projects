using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics
{
    public class VendorCategorySummaryMetric : SummaryMetric
    {
        public VendorCategorySummaryMetric() : base()
        {
        }

        public VendorCategorySummaryMetric(int entityId, DateTime date, int metricTypeId, decimal metricValue) 
        {
            EntityId = entityId;
            MetricTypeId = metricTypeId;
            Date = date;
            Value = metricValue;
        }

        [ForeignKey("EntityId")]
        public virtual VendorCategory Category { get; set; }
    }
}
