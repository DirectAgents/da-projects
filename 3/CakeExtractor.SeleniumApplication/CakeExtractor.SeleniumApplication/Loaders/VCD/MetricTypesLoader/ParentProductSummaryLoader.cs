using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Loaders.VCD.MetricTypesLoader
{
    internal class ParentProductSummaryLoader : BaseVendorItemLoader<ParentProduct, VendorParentProduct, VendorParentProductSummaryMetric>
    {
        private List<VendorCategory> categories;

        private List<VendorSubcategory> subcategories;

        private List<VendorBrand> brands;

        public ParentProductSummaryLoader(List<VendorCategory> categories, List<VendorSubcategory> subcategories,
           List<VendorBrand> brands,Dictionary<string, int> metricTypes)
            : base(metricTypes)
        {
            this.categories = categories;
            this.subcategories = subcategories;
            this.brands = brands;
        }

        protected override Func<VendorParentProduct, bool> GetEntityMappingPredicate(ParentProduct reportEntity, ExtAccount extAccount)
        {
            return db => db.AccountId == extAccount.Id && db.Asin == reportEntity.Asin;
        }

        protected override DbSet<VendorParentProductSummaryMetric> GetSummaryMetricDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.VendorParentProductSummaryMetrics;
        }

        protected override DbSet<VendorParentProduct> GetVendorDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.VendorParentProducts;
        }

        protected override VendorParentProduct MapReportEntityToDbEntity(ParentProduct reportEntity, ExtAccount extAccount)
        {
            var vendorParentProduct = new VendorParentProduct
            {
                Asin = reportEntity.Asin,
                AccountId = extAccount.Id,
            };
            SetCategoryIdIfExists(vendorParentProduct, reportEntity);
            SetSubcategoryIdIfExists(vendorParentProduct, reportEntity);
            SetBrandIdIfExists(vendorParentProduct, reportEntity);
            return vendorParentProduct;
        }

        private void SetCategoryIdIfExists(VendorParentProduct product, ParentProduct reportEntity)
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

        private void SetSubcategoryIdIfExists(VendorParentProduct product, ParentProduct reportEntity)
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

        private void SetBrandIdIfExists(VendorParentProduct product, ParentProduct reportEntity)
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
    }
}
