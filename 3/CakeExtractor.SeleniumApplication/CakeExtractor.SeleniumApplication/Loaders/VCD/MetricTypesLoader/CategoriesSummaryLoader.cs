using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics;
using System.Collections.Generic;
using System.Data.Entity;

namespace CakeExtractor.SeleniumApplication.Loaders.VCD.MetricTypesLoader
{
    internal class CategoriesSummaryLoader : BaseVendorItemLoader<Category, VendorCategory, VendorCategorySummaryMetric>
    {
        public CategoriesSummaryLoader(Dictionary<string, int> metricTypes)
            :base(metricTypes)
        {
        }

        protected override DbSet<VendorCategorySummaryMetric> GetSummaryMetricDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.VendorCategorySummaryMetrics;
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
