using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Vendor
{
    public class VendorCategory : BaseVendorEntity
    {
        public int? BrandId { get; set; }

        [ForeignKey("BrandId")]
        public virtual VendorBrand Brand { get; set; }
    }
}
