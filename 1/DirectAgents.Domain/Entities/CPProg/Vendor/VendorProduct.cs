using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Vendor
{
    public class VendorProduct : BaseVendorEntity
    {
        public string Asin
        {
            get;
            set;
        }

        public int? CategoryId
        {
            get;
            set;
        }

        [ForeignKey("CategoryId")]
        public virtual VendorCategory Category
        {
            get;
            set;
        }

        public int? SubcategoryId
        {
            get;
            set;
        }

        [ForeignKey("SubcategoryId")]
        public virtual VendorSubcategory Subcategory
        {
            get;
            set;
        }

        public int? ParentProductId
        {
            get;
            set;
        }

        [ForeignKey("ParentProductId")]
        public virtual VendorParentProduct ParentProduct
        {
            get;
            set;
        }

        public int? BrandId
        {
            get;
            set;
        }

        [ForeignKey("BrandId")]
        public virtual VendorBrand Brand
        {
            get;
            set;
        }

        public string Ean
        {
            get;
            set;
        }

        public string Upc
        {
            get;
            set;
        }

        public string ApparelSize
        {
            get;
            set;
        }

        public string ApparelSizeWidth
        {
            get;
            set;
        }

        public string Binding
        {
            get;
            set;
        }

        public string Color
        {
            get;
            set;
        }

        public string ModelStyleNumber
        {
            get;
            set;
        }

        public DateTime ReleaseDate
        {
            get;
            set;
        }
    }
}
