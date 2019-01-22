using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Loaders.VCD.MetricTypesLoader
{
    internal class SubcategoriesSummaryLoader : BaseVendorItemLoader<Subcategory, VendorSubcategory, VendorSubcategorySummaryMetric>
    {
        private List<VendorCategory> categories;

        private List<VendorBrand> brands;

        public SubcategoriesSummaryLoader(Dictionary<string, int> metricTypes, List<VendorCategory> categories,
             List<VendorBrand> brands)
            : base(metricTypes)
        {
            this.categories = categories;
            this.brands = brands;
        }

        protected override DbSet<VendorSubcategorySummaryMetric> GetSummaryMetricDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.VendorSubcategorySummaryMetrics;
        }

        protected override DbSet<VendorSubcategory> GetVendorDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.VendorSubcategories;
        }

        protected override VendorSubcategory MapReportEntityToDbEntity(Subcategory reportEntity, ExtAccount extAccount)
        {
            var vendorSubcategory = new VendorSubcategory
            {
                AccountId = extAccount.Id,
                Name = reportEntity.Name,
            };
            SetCategoryIdIfExists(vendorSubcategory, reportEntity);
            SetBrandIdIfExists(vendorSubcategory, reportEntity);
            return vendorSubcategory;
        }

        private void SetCategoryIdIfExists(VendorSubcategory subcategory, Subcategory reportEntity)
        {
            if (!string.IsNullOrEmpty(reportEntity.Category))
            {
                var categoryEntity = categories.FirstOrDefault(cat => cat.Name == reportEntity.Category);
                if (categoryEntity != null)
                {
                    subcategory.CategoryId = categoryEntity.Id;
                }
            }
        }

        private void SetBrandIdIfExists(VendorSubcategory subcategory, Subcategory reportEntity)
        {
            if (!string.IsNullOrEmpty(reportEntity.Brand))
            {
                var brandEntity = brands.FirstOrDefault(brand => brand.Name == reportEntity.Brand);
                if (brandEntity != null)
                {
                    subcategory.BrandId = brandEntity.Id;
                }
            }
        }
    }
}
