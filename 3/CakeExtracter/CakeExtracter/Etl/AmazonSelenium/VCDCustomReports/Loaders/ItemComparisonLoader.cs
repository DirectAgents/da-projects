using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Loaders
{
    /// <summary>
    /// Loader for Item Comparison statistics.
    /// </summary>
    public class ItemComparisonLoader : VcdCustomReportLoader<ItemComparisonProduct, VendorItemComparisonProduct>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemComparisonLoader"/> class.
        /// </summary>
        /// <param name="extAccount">External account.</param>
        public ItemComparisonLoader(ExtAccount extAccount)
            : base(extAccount)
        {
        }

        protected override VendorItemComparisonProduct MapReportEntity(ItemComparisonProduct reportProduct, DateRange dateRange)
        {
            var vendorProduct = new VendorItemComparisonProduct
            {
                Asin = reportProduct.Asin,
                AccountId = extAccount.Id,
                Name = reportProduct.Name,
                No1ComparedAsin = reportProduct.No1ComparedAsin,
                No1ComparedProductTitle = reportProduct.No1ComparedProductTitle,
                No1ComparedPercent = reportProduct.No1ComparedPercent,
                No2ComparedAsin = reportProduct.No2ComparedAsin,
                No2ComparedProductTitle = reportProduct.No2ComparedProductTitle,
                No2ComparedPercent = reportProduct.No2ComparedPercent,
                No3ComparedAsin = reportProduct.No3ComparedAsin,
                No3ComparedProductTitle = reportProduct.No3ComparedProductTitle,
                No3ComparedPercent = reportProduct.No3ComparedPercent,
                No4ComparedAsin = reportProduct.No4ComparedAsin,
                No4ComparedProductTitle = reportProduct.No4ComparedProductTitle,
                No4ComparedPercent = reportProduct.No4ComparedPercent,
                No5ComparedAsin = reportProduct.No5ComparedAsin,
                No5ComparedProductTitle = reportProduct.No5ComparedProductTitle,
                No5ComparedPercent = reportProduct.No5ComparedPercent,
                Date = dateRange.ToDate,
            };
            return vendorProduct;
        }

        protected override void RemoveOldData(VcdCustomReportData<ItemComparisonProduct> item)
        {

            Logger.Info(
                accountId,
                $"The cleaning of ItemComparison for account ({accountId}) has begun - from {item.ReportDateRange.FromDate} to {item.ReportDateRange.ToDate}.");
            SafeContextWrapper.TryMakeTransaction(
                (ClientPortalProgContext db) =>
                {
                    db.VendorMarketBasketAnalysisProductMetrics.Where(x =>
                    x.Date >= item.ReportDateRange.FromDate &&
                    x.Date <= item.ReportDateRange.ToDate &&
                    x.AccountId == accountId).DeleteFromQuery();
                }, "DeleteFromQuery");
            Logger.Info(
                accountId,
                $"The cleaning of ItemComparison data for account ({accountId}) is over - from {item.ReportDateRange.FromDate} to {item.ReportDateRange.ToDate}.");
        }

        protected override void BulkSaveData(VcdCustomReportData<ItemComparisonProduct> item)
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                var dateRange = item.ReportDateRange;
                var products = item.Products.Select(x => MapReportEntity(x, dateRange));
                dbContext.BulkInsert(products);
            }
        }
    }
}
