using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Loaders
{
    public class GeographicSalesInsightsLoader : VcdCustomReportLoader<GeoSalesProduct, VendorGeographicSalesInsightsProduct>
    {
        public GeographicSalesInsightsLoader(ExtAccount extAccount)
            : base(extAccount)
        {
        }

        protected override VendorGeographicSalesInsightsProduct MapReportEntity(GeoSalesProduct reportProduct, DateRange dateRange)
        {
            var vendorProduct = new VendorGeographicSalesInsightsProduct
            {
                Asin = reportProduct.Asin,
                AccountId = extAccount.Id,
                Name = reportProduct.Name,
                State = reportProduct.State,
                Region = reportProduct.Region,
                City = reportProduct.City,
                Zip = reportProduct.Zip,
                Author = reportProduct.Author,
                Isbn13 = reportProduct.Isbn13,
                AverageShippedPrice = reportProduct.AverageShippedPrice,
                AverageShippedPricePriorPeriodPercentChange = reportProduct.AverageShippedPricePriorPeriodPercentChange,
                AverageShippedPricePriorYearPercentChange = reportProduct.AverageShippedPricePriorYearPercentChange,
                ShippedCogsPercentOfTotal = reportProduct.ShippedCogsPercentOfTotal,
                ShippedCogsPriorPeriodPercentChange = reportProduct.ShippedCogsPriorPeriodPercentChange,
                ShippedCogsPriorYearPercentChange = reportProduct.ShippedRevenuePriorYearPercentChange,
                ShippedRevenuePercentOfTotal = reportProduct.ShippedRevenuePercentOfTotal,
                ShippedRevenuePriorPeriodPercentChange = reportProduct.ShippedRevenuePriorPeriodPercentChange,
                ShippedRevenuePriorYearPercentChange = reportProduct.ShippedRevenuePriorYearPercentChange,
                ShippedUnitsPercentOfTotal = reportProduct.ShippedUnitsPercentOfTotal,
                ShippedUnitsPriorPeriodPercentChange = reportProduct.ShippedUnitsPriorPeriodPercentChange,
                ShippedUnitsPriorYearPercentChange = reportProduct.ShippedUnitsPriorYearPercentChange,
                Date = dateRange.ToDate,
            };
            return vendorProduct;
        }

        protected override void RemoveOldData(VcdCustomReportData<GeoSalesProduct> item)
        {

            Logger.Info(
                accountId,
                $"The cleaning of GeographicSalesInsights for account ({accountId}) has begun - from {item.ReportDateRange.FromDate} to {item.ReportDateRange.ToDate}.");
            SafeContextWrapper.TryMakeTransaction(
                (ClientPortalProgContext db) =>
                {
                    db.VendorGeographicSalesInsightsProducts.Where(x =>
                    x.Date >= item.ReportDateRange.FromDate &&
                    x.Date <= item.ReportDateRange.ToDate &&
                    x.AccountId == accountId).DeleteFromQuery();
                }, "DeleteFromQuery");
            Logger.Info(
                accountId,
                $"The cleaning of GeographicSalesInsights data for account ({accountId}) is over - from {item.ReportDateRange.FromDate} to {item.ReportDateRange.ToDate}.");
        }
    }
}
