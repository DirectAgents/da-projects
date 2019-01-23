using System;
using System.Collections.Generic;
using System.Data.Entity;
using CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics;

namespace CakeExtractor.SeleniumApplication.Loaders.VCD.MetricTypesLoader
{
    internal class BrandsSummaryLoader : BaseVendorItemLoader<Brand, VendorBrand, VendorBrandSummaryMetric>
    {
        public BrandsSummaryLoader(Dictionary<string, int> metricTypes)
            : base(metricTypes)
        {
        }

        protected override Func<VendorBrand, bool> GetEntityMappingPredicate(Brand reportEntity, ExtAccount extAccount)
        {
            return base.GetEntityMappingPredicate(reportEntity, extAccount);
        }

        protected override DbSet<VendorBrandSummaryMetric> GetSummaryMetricDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.VendorBrandSummaryMetrics;
        }

        protected override DbSet<VendorBrand> GetVendorDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.VendorBrands;
        }

        protected override VendorBrand MapReportEntityToDbEntity(Brand reportEntity, ExtAccount extAccount)
        {
            var vendorBrand = new VendorBrand
            {
                AccountId = extAccount.Id,
                Name = reportEntity.Name
            };
            return vendorBrand;
        }
    }
}
