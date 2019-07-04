using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.VCD.Models;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Loaders.MetricTypesLoader
{
    internal class SubcategoriesSummaryLoader : BaseVendorItemLoader<Subcategory, VendorSubcategory, VendorSubcategorySummaryMetric>
    {
        private readonly List<VendorCategory> categories;
        private readonly List<VendorBrand> brands;

        public SubcategoriesSummaryLoader(
            Dictionary<string, int> metricTypes, List<VendorCategory> categories, List<VendorBrand> brands)
            : base(metricTypes)
        {
            this.categories = categories;
            this.brands = brands;
        }

        protected override VendorSubcategory MapReportEntityToDbEntity(
            Subcategory reportEntity, ExtAccount extAccount)
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
            if (string.IsNullOrEmpty(reportEntity.Category))
            {
                return;
            }
            var categoryEntity = categories.FirstOrDefault(cat => cat.Name == reportEntity.Category);
            if (categoryEntity != null)
            {
                subcategory.CategoryId = categoryEntity.Id;
            }
        }

        private void SetBrandIdIfExists(VendorSubcategory subcategory, Subcategory reportEntity)
        {
            if (string.IsNullOrEmpty(reportEntity.Brand))
            {
                return;
            }
            var brandEntity = brands.FirstOrDefault(brand => brand.Name == reportEntity.Brand);
            if (brandEntity != null)
            {
                subcategory.BrandId = brandEntity.Id;
            }
        }
    }
}