using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics
{
    public class VendorParentProductSummaryMetric : SummaryMetric
    {
        public VendorParentProductSummaryMetric() : base()
        {
        }

        public VendorParentProductSummaryMetric(int entityId, DateTime date, int metricTypeId, decimal metricValue) 
        {
            EntityId = entityId;
            MetricTypeId = metricTypeId;
            Date = date;
            Value = metricValue;
        }

        [ForeignKey("EntityId")]
        public virtual VendorParentProduct ParentProduct { get; set; }
    }
}
