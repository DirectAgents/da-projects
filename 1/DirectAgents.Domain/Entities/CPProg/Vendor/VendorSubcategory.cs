using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Vendor
{
    public class VendorSubcategory: BaseVendorEntity
    {
        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual VendorCategory Category { get; set; }
    }
}
