using CakeExtractor.SeleniumApplication.Loaders.VCD.Constants;
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
    internal class SubcategoriesSummaryLoader : BaseVendorItemLoader<Subcategory, VendorSubcategory, VendorSubcategorySummaryMetric>
    {
        private List<VendorCategory> categories;

        public SubcategoriesSummaryLoader(List<VendorCategory> categories, Dictionary<string, int> metricTypes)
            :base(metricTypes)
        {
            this.categories = categories;
        }

        protected override DbSet<VendorSubcategorySummaryMetric> GetSummaryMetricDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.VendorSubcategorySummaryMetrics;
        }

        protected override List<VendorSubcategorySummaryMetric> GetSummaryMetricEntities(Subcategory reportEntity, VendorSubcategory dbEntity, DateTime date)
        {
            return new List<VendorSubcategorySummaryMetric>
            {
                new VendorSubcategorySummaryMetric(dbEntity.Id, date,
                    metricTypes[VendorCentralDataLoadingConstants.ShippedUnitsMetricName], reportEntity.ShippedUnits),
                new VendorSubcategorySummaryMetric(dbEntity.Id, date,
                    metricTypes[VendorCentralDataLoadingConstants.OrderedUnitsMetricName], reportEntity.OrderedUnits),
                new VendorSubcategorySummaryMetric(dbEntity.Id, date,
                    metricTypes[VendorCentralDataLoadingConstants.ShippedRevenueMetricName], reportEntity.ShippedRevenue)
            };
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
            return vendorSubcategory;
        }

        private void SetCategoryIdIfExists(VendorSubcategory subcategory, Subcategory reportEntity)
        {
            if (!string.IsNullOrEmpty(reportEntity.CategoryName))
            {
                var categoryEntity = categories.FirstOrDefault(cat => cat.Name == reportEntity.CategoryName);
                if (categoryEntity != null)
                {
                    subcategory.CategoryId = categoryEntity.Id;
                }
            }
        }
    }
}
