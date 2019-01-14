using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics
{
    public class VendorCategorySummaryMetric : SummaryMetric
    {
        [ForeignKey("EntityId")]
        public virtual VendorCategory Category { get; set; }
    }
}
