using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.VCD.Models;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Loaders.MetricTypesLoader
{
    internal class ProductsSummaryLoader : BaseVendorItemLoader<Product, VendorProduct, VendorProductSummaryMetric>
    {
        private readonly List<VendorCategory> categories;
        private readonly List<VendorSubcategory> subcategories;
        private readonly List<VendorBrand> brands;
        private readonly List<VendorParentProduct> parentProducts;

        public ProductsSummaryLoader(List<VendorCategory> categories, List<VendorSubcategory> subcategories,
           List<VendorBrand> brands, List<VendorParentProduct> parentProducts, Dictionary<string, int> metricTypes)
            : base(metricTypes)
        {
            this.categories = categories;
            this.subcategories = subcategories;
            this.brands = brands;
            this.parentProducts = parentProducts;
        }

        protected override Func<VendorProduct, bool> GetEntityMappingPredicate(Product reportEntity, ExtAccount extAccount)
        {
            return db => db.Name == reportEntity.Name && db.AccountId == extAccount.Id && db.Asin == reportEntity.Asin;
        }
        
        protected override VendorProduct MapReportEntityToDbEntity(Product reportEntity, ExtAccount extAccount)
        {
            var vendorProduct = new VendorProduct
            {
                Asin = reportEntity.Asin,
                AccountId = extAccount.Id,
                Name = reportEntity.Name,
                ApparelSize = reportEntity.ApparelSize,
                ApparelSizeWidth = reportEntity.ApparelSizeWidth,
                Binding = reportEntity.Binding,
                Color = reportEntity.Color,
                Ean = reportEntity.Ean,
                Upc = reportEntity.Upc,
                ModelStyleNumber = reportEntity.ModelStyleNumber,
                ReleaseDate = reportEntity.ReleaseDate < SqlDateTime.MinValue.Value ?
                    SqlDateTime.MinValue.Value : reportEntity.ReleaseDate
            };
            SetCategoryIdIfExists(vendorProduct, reportEntity);
            SetSubcategoryIdIfExists(vendorProduct, reportEntity);
            SetBrandIdIfExists(vendorProduct, reportEntity);
            SetParentProductIdIfExists(vendorProduct, reportEntity);
            return vendorProduct;
        }

        private void SetCategoryIdIfExists(VendorProduct product, Product reportEntity)
        {
            if (string.IsNullOrEmpty(reportEntity.Category))
                return;

            var categoryEntity = categories.FirstOrDefault(cat => cat.Name == reportEntity.Category);
            if (categoryEntity != null)
            {
                product.CategoryId = categoryEntity.Id;
            }
        }

        private void SetSubcategoryIdIfExists(VendorProduct product, Product reportEntity)
        {
            if (string.IsNullOrEmpty(reportEntity.Subcategory))
                return;

            var subcategoryEntity = subcategories.FirstOrDefault(subCat => subCat.Name == reportEntity.Subcategory);
            if (subcategoryEntity != null)
            {
                product.SubcategoryId = subcategoryEntity.Id;
            }
        }

        private void SetBrandIdIfExists(VendorProduct product, Product reportEntity)
        {
            if (string.IsNullOrEmpty(reportEntity.Brand))
                return;

            var brandEntity = brands.FirstOrDefault(brand => brand.Name == reportEntity.Brand);
            if (brandEntity != null)
            {
                product.BrandId = brandEntity.Id;
            }
        }

        private void SetParentProductIdIfExists(VendorProduct product, Product reportEntity)
        {
            if (string.IsNullOrEmpty(reportEntity.ParentAsin))
                return;

            var parentProductEntity = parentProducts.FirstOrDefault(pp => pp.Asin == reportEntity.ParentAsin);
            if (parentProductEntity != null)
            {
                product.ParentProductId = parentProductEntity.Id;
            }
        }
    }
}
