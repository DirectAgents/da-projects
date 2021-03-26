using System;
using System.Collections.Generic;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Loaders
{
    /// <summary>
    /// General loader for Amazon Vendor Central custom statistics.
    /// </summary>
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

        /// <summary>
        /// Removes old data from database.
        /// </summary>
        /// <param name="item">Report data item to remove.</param>
        protected abstract void RemoveOldData(VcdCustomReportData<TProduct> item);

        /// <summary>
        /// Maps data from product to database product entity.
        /// </summary>
        /// <param name="reportProduct">Product to map.</param>
        /// <param name="dateRange">Date range for product</param>
        protected abstract TProductDbEntity MapReportEntity(TProduct reportProduct, DateRange dateRange);

        protected abstract void BulkSaveData(VcdCustomReportData<TProduct> item);

        /// <inheritdoc/>
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
    }
}
