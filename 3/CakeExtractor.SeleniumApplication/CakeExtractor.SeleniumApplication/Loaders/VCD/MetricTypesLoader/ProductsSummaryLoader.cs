using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Loaders.VCD.MetricTypesLoader
{
    internal class ProductsSummaryLoader : BaseVendorItemLoader<Product, VendorProduct, VendorProductSummaryMetric>
    {
        private List<VendorCategory> categories;

        private List<VendorSubcategory> subcategories;

        private List<VendorBrand> brands;

        private List<VendorParentProduct> parentProducts;

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

        protected override DbSet<VendorProductSummaryMetric> GetSummaryMetricDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.VendorProductSummaryMetrics;
        }

        protected override DbSet<VendorProduct> GetVendorDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.VendorProducts;
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
            if (!string.IsNullOrEmpty(reportEntity.Category))
            {
                var categoryEntity = categories.FirstOrDefault(cat => cat.Name == reportEntity.Category);
                if (categoryEntity != null)
                {
                    product.CategoryId = categoryEntity.Id;
                }
            }
        }

        private void SetSubcategoryIdIfExists(VendorProduct product, Product reportEntity)
        {
            if (!string.IsNullOrEmpty(reportEntity.Subcategory))
            {
                var subcategoryEntity = subcategories.FirstOrDefault(subCat => subCat.Name == reportEntity.Subcategory);
                if (subcategoryEntity != null)
                {
                    product.SubcategoryId = subcategoryEntity.Id;
                }
            }
        }

        private void SetBrandIdIfExists(VendorProduct product, Product reportEntity)
        {
            if (!string.IsNullOrEmpty(reportEntity.Brand))
            {
                var brandEntity = brands.FirstOrDefault(brand => brand.Name == reportEntity.Brand);
                if (brandEntity != null)
                {
                    product.BrandId = brandEntity.Id;
                }
            }
        }

        private void SetParentProductIdIfExists(VendorProduct product, Product reportEntity)
        {
            if (!string.IsNullOrEmpty(reportEntity.ParentAsin))
            {
                var parentProductEntity = parentProducts.FirstOrDefault(pp => pp.Asin == reportEntity.ParentAsin);
                if (parentProductEntity != null)
                {
                    product.ParentProductId = parentProductEntity.Id;
                }
            }
        }
    }
}
