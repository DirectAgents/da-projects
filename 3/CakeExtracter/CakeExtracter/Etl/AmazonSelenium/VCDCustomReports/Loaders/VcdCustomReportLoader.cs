using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Loaders
{
    public abstract class VcdCustomReportLoader<TProduct, TProductDbEntity> : Loader<VcdCustomReportData<TProduct>>
        where TProduct : VcdCustomProduct
        where TProductDbEntity : BaseVendorEntity
    {
        protected readonly ExtAccount extAccount;

        /// <summary>
        /// Initializes a new instance of the <see cref="VcdCustomReportLoader"/> class.
        /// </summary>
        /// <param name="extAccount">External account</param>
        protected VcdCustomReportLoader(ExtAccount extAccount)
        {
            this.extAccount = extAccount;
        }

        protected abstract void RemoveOldData(VcdCustomReportData<TProduct> item);

        protected abstract TProductDbEntity MapReportEntity(TProduct reportProduct, DateRange dateRange);

        protected override int Load(List<VcdCustomReportData<TProduct>> items)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    foreach (var item in items)
                    {
                        try
                        {
                            RemoveOldData(item);
                            BulkSaveData(item);
                        }
                        catch (Exception e)
                        {
                            Logger.Error(
                                extAccount.Id,
                                new Exception($"Could not load {item.ReportType} data for period {item.PeriodType}. Details: {e}", e));
                        }
                    }
                },
                extAccount.Id,
                "VCD Custom Data Loader",
                AmazonJobOperations.LoadSummaryItemsData);
            return items.Count;
        }

        private void BulkSaveData(VcdCustomReportData<TProduct> item)
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                var products = item.Products.Select(MapReportEntity);
                dbContext.BulkInsert(products);
            }
        }
    }
}