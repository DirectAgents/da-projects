using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Loaders.VCD.MetricTypesLoader
{
    internal class CategoriesSummaryLoader : BaseVendorItemLoader<Category, VendorCategory, VendorCategorySummaryMetric>
    {
        private readonly List<VendorBrand> brands;

        public CategoriesSummaryLoader(Dictionary<string, int> metricTypes, List<VendorBrand> brands)
            :base(metricTypes)
        {
            this.brands = brands;
        }
        
        protected override VendorCategory MapReportEntityToDbEntity(Category reportEntity, ExtAccount extAccount)
        {
            var dbCategory = new VendorCategory
            {
                AccountId = extAccount.Id,
                Name = reportEntity.Name
            };
            SetBrandIdIfExists(dbCategory, reportEntity);
            return dbCategory;
        }

        private void SetBrandIdIfExists(VendorCategory category, Category reportEntity)
        {
            if (string.IsNullOrEmpty(reportEntity.Brand))
                return;

            var brandEntity = brands.FirstOrDefault(brand => brand.Name == reportEntity.Brand);
            if (brandEntity != null)
            {
                category.BrandId = brandEntity.Id;
            }
        }
    }
}
