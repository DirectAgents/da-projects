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
    /// Loader for Alternative Purchase statistics.
    /// </summary>
    public class AlternativePurchaseLoader : VcdCustomReportLoader<AlternativePurchaseProduct, VendorAlternativePurchaseProduct>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlternativePurchaseLoader"/> class.
        /// </summary>
        /// <param name="extAccount">External account.</param>
        public AlternativePurchaseLoader(ExtAccount extAccount)
            : base(extAccount)
        {
        }

        protected override VendorAlternativePurchaseProduct MapReportEntity(AlternativePurchaseProduct reportProduct, DateRange dateRange)
        {
            var vendorProduct = new VendorAlternativePurchaseProduct
            {
                Asin = reportProduct.Asin,
                AccountId = extAccount.Id,
                Name = reportProduct.Name,
                No1PurchasedAsin = reportProduct.No1PurchasedAsin,
                No1PurchasedProductTitle = reportProduct.No1PurchasedProductTitle,
                No1PurchasedPercent = reportProduct.No1PurchasedPercent,
                No2PurchasedAsin = reportProduct.No2PurchasedAsin,
                No2PurchasedProductTitle = reportProduct.No2PurchasedProductTitle,
                No2PurchasedPercent = reportProduct.No2PurchasedPercent,
                No3PurchasedAsin = reportProduct.No3PurchasedAsin,
                No3PurchasedProductTitle = reportProduct.No3PurchasedProductTitle,
                No3PurchasedPercent = reportProduct.No3PurchasedPercent,
                No4PurchasedAsin = reportProduct.No4PurchasedAsin,
                No4PurchasedProductTitle = reportProduct.No4PurchasedProductTitle,
                No4PurchasedPercent = reportProduct.No4PurchasedPercent,
                No5PurchasedAsin = reportProduct.No5PurchasedAsin,
                No5PurchasedProductTitle = reportProduct.No5PurchasedProductTitle,
                No5PurchasedPercent = reportProduct.No5PurchasedPercent,
                Date = dateRange.ToDate,
            };
            return vendorProduct;
        }

        protected override void RemoveOldData(VcdCustomReportData<AlternativePurchaseProduct> item)
        {

            Logger.Info(
                accountId,
                $"The cleaning of AlternativePurchase for account ({accountId}) has begun - from {item.ReportDateRange.FromDate} to {item.ReportDateRange.ToDate}.");
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
                $"The cleaning of AlternativePurchase data for account ({accountId}) is over - from {item.ReportDateRange.FromDate} to {item.ReportDateRange.ToDate}.");
        }

        protected override void BulkSaveData(VcdCustomReportData<AlternativePurchaseProduct> item)
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
