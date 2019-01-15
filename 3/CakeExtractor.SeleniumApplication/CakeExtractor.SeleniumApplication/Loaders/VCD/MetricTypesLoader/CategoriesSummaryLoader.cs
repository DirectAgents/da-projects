using CakeExtractor.SeleniumApplication.Loaders.VCD.Constants;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace CakeExtractor.SeleniumApplication.Loaders.VCD.MetricTypesLoader
{
    internal class CategoriesSummaryLoader : BaseVendorItemLoader<Category, VendorCategory, VendorCategorySummaryMetric>
    {
        protected override DbSet<VendorCategorySummaryMetric> GetSummaryMetricDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.VendorCategorySummaryMetrics;
        }

        protected override List<VendorCategorySummaryMetric> GetSummaryMetricEntities(Category reportEntity, VendorCategory dbEntity, DateTime date, Dictionary<string, MetricType> metricTypesDictionary)
        {
            return new List<VendorCategorySummaryMetric>
            {
                new VendorCategorySummaryMetric(dbEntity.Id, date,
                    metricTypesDictionary[VendorCentralDataLoadingConstants.ShippedUnitsMetricName], reportEntity.ShippedUnits),
                new VendorCategorySummaryMetric(dbEntity.Id, date,
                    metricTypesDictionary[VendorCentralDataLoadingConstants.OrderedUnitsMetricName], reportEntity.OrderedUnits),
                new VendorCategorySummaryMetric(dbEntity.Id, date,
                    metricTypesDictionary[VendorCentralDataLoadingConstants.ShippedRevenueMetricName], reportEntity.ShippedRevenue)
            };
        }

        protected override DbSet<VendorCategory> GetVendorDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.VendorCategories;
        }

        protected override VendorCategory MapReportEntityToDbEntity(Category reportEntity, ExtAccount extAccount)
        {
            return new VendorCategory
            {
                AccountId = extAccount.Id,
                Name = reportEntity.Name
            };
        }
    }
}
