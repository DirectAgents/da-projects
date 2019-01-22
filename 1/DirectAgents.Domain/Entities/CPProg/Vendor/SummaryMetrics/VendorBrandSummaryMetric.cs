using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics
{
    public class VendorBrandSummaryMetric : SummaryMetric
    {
        public VendorBrandSummaryMetric() : base()
        {
        }

        public VendorBrandSummaryMetric(int entityId, DateTime date, int metricTypeId, decimal metricValue) 
        {
            EntityId = entityId;
            MetricTypeId = metricTypeId;
            Date = date;
            Value = metricValue;
        }

        [ForeignKey("EntityId")]
        public virtual VendorBrand Brand { get; set; }
    }
}
