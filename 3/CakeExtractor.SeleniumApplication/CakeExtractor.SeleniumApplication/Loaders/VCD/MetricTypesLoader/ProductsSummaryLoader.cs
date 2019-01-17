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
    internal class ProductsSummaryLoader : BaseVendorItemLoader<Product, VendorProduct, VendorProductSummaryMetric>
    {
        private List<VendorCategory> categories;

        private List<VendorSubcategory> subcategories;

        public ProductsSummaryLoader(List<VendorCategory> categories, List<VendorSubcategory> subcategories, Dictionary<string, int> metricTypes)
            : base(metricTypes)
        {
            this.categories = categories;
            this.subcategories = subcategories;
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
                Name = reportEntity.Name
            };
            SetCategoryIdIfExists(vendorProduct, reportEntity);
            SetSubcategoryIdIfExists(vendorProduct, reportEntity);
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
    }
}
