using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics
{
    public class VendorProductSummaryMetric : SummaryMetric
    {
        [ForeignKey("EntityId")]
        public virtual VendorProduct Product { get; set; }
    }
}
