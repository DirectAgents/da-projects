using System.Collections.Generic;
using CakeExtracter.Etl.AmazonSelenium.VCD.Models;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.SummaryMetrics;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Loaders.MetricTypesLoader
{
    internal class BrandsSummaryLoader : BaseVendorItemLoader<Brand, VendorBrand, VendorBrandSummaryMetric>
    {
        public BrandsSummaryLoader(Dictionary<string, int> metricTypes)
            : base(metricTypes)
        {
        }

        protected override VendorBrand MapReportEntityToDbEntity(Brand reportEntity, ExtAccount extAccount)
        {
            return new VendorBrand
            {
                AccountId = extAccount.Id,
                Name = reportEntity.Name,
            };
        }
    }
}
