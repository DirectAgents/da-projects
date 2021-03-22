using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Contexts;
using CakeExtracter.Logging.TimeWatchers.Amazon;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Loaders
{
    /// <summary>
    /// Loader for Geographic Sales Insights statistics.
    /// </summary>
    internal class GeographicSalesInsightsLoader : Loader<VcdGeographicSalesInsightsReportData>
    {
        private readonly ExtAccount extAccount;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographicSalesInsightsLoader"/> class.
        /// </summary>
        /// <param name="extAccount">External account</param>
        public GeographicSalesInsightsLoader(ExtAccount extAccount)
        {
            this.extAccount = extAccount;
        }

        /// <summary>
        /// Loads Amazon Vendor Central statistics.
        /// </summary>
        /// <param name="items">List of Amazon Vendor Central stats.</param>
        /// <returns>Count of loading items.</returns>
        protected override int Load(List<VcdGeographicSalesInsightsReportData> items)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    foreach (var item in items)
                    {
                        try
                        {
                            LoadGeographicSalesInsightsData(item);
                        }
                        catch (Exception e)
                        {
                            Logger.Error(
                                extAccount.Id,
                                new Exception($"Could not load data for for date {item.Date}. Details: {e}", e));
                        }
                    }
                },
                extAccount.Id,
                "VCD Data Loader",
                AmazonJobOperations.LoadSummaryItemsData);
            return items.Count;
        }

        public List<VendorGeographicSalesInsightsProduct> LoadGeographicSalesInsightsData(VcdGeographicSalesInsightsReportData reportData)
        {
            var dbProducts = EnsureVendorEntitiesInDataBase(reportData.Products, extAccount, reportData.Date);
            Logger.Info(extAccount.Id, "Amazon VCD, Finished loading NetPpm data. Loaded metrics of {0} products", dbProducts.Count);
            return dbProducts;
        }

        public List<VendorGeographicSalesInsightsProduct> EnsureVendorEntitiesInDataBase(
            List<GeographicSalesInsightsProduct> reportEntities, ExtAccount account, DateTime date)
        {
            var itemsToBeAdded = new List<VendorGeographicSalesInsightsProduct>();
            var allItemsFromReport = new List<VendorGeographicSalesInsightsProduct>();
            using (var dbContext = new ClientPortalProgContext())
            {
                var accountRelatedVendorEntities =
                    dbContext.Set<VendorGeographicSalesInsightsProduct>().Where(e => e.AccountId == account.Id).ToList();
                reportEntities.ForEach(reportEntity =>
                {
                    var correspondingDbEntity =
                    accountRelatedVendorEntities.FirstOrDefault(GetEntityMappingPredicate(reportEntity, account));
                    if (correspondingDbEntity == null)
                    {
                        correspondingDbEntity = MapReportEntityToDbEntity(reportEntity, account, date);
                        itemsToBeAdded.Add(correspondingDbEntity);
                    }
                    allItemsFromReport.Add(correspondingDbEntity);
                });
                dbContext.Set<VendorGeographicSalesInsightsProduct>().AddRange(itemsToBeAdded);
                dbContext.SaveChanges();
                var allItems = accountRelatedVendorEntities.Concat(itemsToBeAdded).ToList();
                return allItems;
            }
        }

        private Func<VendorGeographicSalesInsightsProduct, bool> GetEntityMappingPredicate(
            GeographicSalesInsightsProduct reportEntity, ExtAccount extAccount)
        {
            return dbEntity => dbEntity.Name == reportEntity.Name && dbEntity.AccountId == extAccount.Id;
        }

        private VendorGeographicSalesInsightsProduct MapReportEntityToDbEntity(
            GeographicSalesInsightsProduct reportEntity, ExtAccount extAccount, DateTime date)
        {
            var vendorProduct = new VendorGeographicSalesInsightsProduct
            {
                Asin = reportEntity.Asin,
                AccountId = extAccount.Id,
                Name = reportEntity.Name,
                State = reportEntity.State,
                Region = reportEntity.Region,
                City = reportEntity.City,
                Zip = reportEntity.Zip,
                Author = reportEntity.Author,
                Isbn13 = reportEntity.Isbn13,
                AverageShippedPrice = reportEntity.AverageShippedPrice,
                AverageShippedPricePriorPeriodPercentChange = reportEntity.AverageShippedPricePriorPeriodPercentChange,
                AverageShippedPricePriorYearPercentChange = reportEntity.AverageShippedPricePriorYearPercentChange,
                ShippedCogsPercentOfTotal = reportEntity.ShippedCogsPercentOfTotal,
                ShippedCogsPriorPeriodPercentChange = reportEntity.ShippedCogsPriorPeriodPercentChange,
                ShippedCogsPriorYearPercentChange = reportEntity.ShippedRevenuePriorYearPercentChange,
                ShippedRevenuePercentOfTotal = reportEntity.ShippedRevenuePercentOfTotal,
                ShippedRevenuePriorPeriodPercentChange = reportEntity.ShippedRevenuePriorPeriodPercentChange,
                ShippedRevenuePriorYearPercentChange = reportEntity.ShippedRevenuePriorYearPercentChange,
                ShippedUnitsPercentOfTotal = reportEntity.ShippedUnitsPercentOfTotal,
                ShippedUnitsPriorPeriodPercentChange = reportEntity.ShippedUnitsPriorPeriodPercentChange,
                ShippedUnitsPriorYearPercentChange = reportEntity.ShippedUnitsPriorYearPercentChange,
                Date = date,
            };
            return vendorProduct;
        }
    }
}