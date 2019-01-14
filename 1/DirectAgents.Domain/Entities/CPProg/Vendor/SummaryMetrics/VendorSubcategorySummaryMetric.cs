using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics
{
    public class VendorSubcategorySummaryMetric : SummaryMetric
    {
        [ForeignKey("EntityId")]
        public virtual VendorSubcategory Subcategory { get; set; }
    }
}
