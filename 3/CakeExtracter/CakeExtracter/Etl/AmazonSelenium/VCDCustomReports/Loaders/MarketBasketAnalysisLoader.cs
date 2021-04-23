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
    /// Loader for Market Basket Analysis statistics.
    /// </summary>
    public class MarketBasketAnalysisLoader : VcdCustomReportLoader<MarketBasketAnalysisProduct, VendorMarketBasketAnalysisProduct>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarketBasketAnalysisLoader"/> class.
        /// </summary>
        /// <param name="extAccount">External account.</param>
        public MarketBasketAnalysisLoader(ExtAccount extAccount)
            : base(extAccount)
        {
        }

        protected override VendorMarketBasketAnalysisProduct MapReportEntity(MarketBasketAnalysisProduct reportProduct, DateRange dateRange)
        {
            var vendorProduct = new VendorMarketBasketAnalysisProduct
            {
                Asin = reportProduct.Asin,
                AccountId = extAccount.Id,
                Name = reportProduct.Name,
                N1PurchasedAsin = reportProduct.N1PurchasedAsin,
                N1PurchasedTitle = reportProduct.N1PurchasedTitle,
                N1Combination = reportProduct.N1Combination,
                N2PurchasedAsin = reportProduct.N2PurchasedAsin,
                N2PurchasedTitle = reportProduct.N2PurchasedTitle,
                N2Combination = reportProduct.N2Combination,
                N3PurchasedAsin = reportProduct.N3PurchasedAsin,
                N3PurchasedTitle = reportProduct.N3PurchasedTitle,
                N3Combination = reportProduct.N3Combination,
                Date = dateRange.ToDate,
            };
            return vendorProduct;
        }

        protected override void RemoveOldData(VcdCustomReportData<MarketBasketAnalysisProduct> item)
        {

            Logger.Info(
                accountId,
                $"The cleaning of MarketBasketAnalysis for account ({accountId}) has begun - from {item.ReportDateRange.FromDate} to {item.ReportDateRange.ToDate}.");
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
                $"The cleaning of MarketBasketAnalysis data for account ({accountId}) is over - from {item.ReportDateRange.FromDate} to {item.ReportDateRange.ToDate}.");
        }

        protected override void BulkSaveData(VcdCustomReportData<MarketBasketAnalysisProduct> item)
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
