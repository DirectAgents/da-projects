using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.RepeatPurchaseBehavior;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Loaders
{
    /// <summary>
    /// Loader for RepearPurchaseBehavior statistics.
    /// </summary>
    internal class RepeatPurchaseBehaviorLoader : Loader<RepeatPurchaseBehaviorReportData>
    {
        private readonly string period;
        private readonly ExtAccount extAccount;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatPurchaseBehaviorLoader"/> class.
        /// </summary>
        /// <param name="extAccount">External account</param>
        /// <param name="period">Period for which report is loaded.</param>
        public RepeatPurchaseBehaviorLoader(ExtAccount extAccount, string period)
        {
            this.extAccount = extAccount;
            this.period = period;
        }

        public List<T> EnsureVendorEntitiesInDataBase<T>(
            List<RepeatPurchaseBehaviorProduct> reportEntities, ExtAccount account, DateTime startDate, DateTime endDate)
            where T : VendorRepeatPurchaseBehaviorProduct
        {
            var itemsToBeAdded = new List<T>();
            var allItemsFromReport = new List<T>();
            using (var dbContext = new ClientPortalProgContext())
            {
                var accountRelatedVendorEntities =
                    dbContext.Set<T>().Where(e => e.AccountId == account.Id).ToList();
                reportEntities.ForEach(reportEntity =>
                {
                    var correspondingDbEntity =
                    accountRelatedVendorEntities.FirstOrDefault(GetEntityMappingPredicate<T>(reportEntity, account));
                    if (correspondingDbEntity == null)
                    {
                        switch (period)
                        {
                            case "MONTHLY":
                                correspondingDbEntity = MapReportEntityToDbEntityMonthly(reportEntity, account, startDate, endDate) as T;
                                break;
                            case "QUATERLY":
                                correspondingDbEntity = MapReportEntityToDbEntityQuaterly(reportEntity, account, startDate, endDate) as T;
                                break;
                        }
                        itemsToBeAdded.Add(correspondingDbEntity);
                    }
                    allItemsFromReport.Add(correspondingDbEntity);
                });
                dbContext.Set<T>().AddRange(itemsToBeAdded);
                dbContext.SaveChanges();
                var allItems = accountRelatedVendorEntities.Concat(itemsToBeAdded).ToList();
                return allItems;
            }
        }

        public List<T> LoadRepeatPurchaseBehaviorData<T>(RepeatPurchaseBehaviorReportData reportData)
            where T : VendorRepeatPurchaseBehaviorProduct
        {
            var dbProducts = EnsureVendorEntitiesInDataBase<T>(reportData.Products, extAccount, reportData.StartDate, reportData.EndDate);
            Logger.Info(extAccount.Id, "Amazon VCD, Finished loading RepeatPurchaseBehavior data. Loaded metrics of {0} products", dbProducts.Count);
            return dbProducts;
        }

        /// <summary>
        /// Loads Amazon Vendor Central statistics.
        /// </summary>
        /// <param name="items">List of Amazon Vendor Central stats.</param>
        /// <returns>Count of loading items.</returns>
        protected override int Load(List<RepeatPurchaseBehaviorReportData> items)
        {
            AmazonTimeTracker.Instance.ExecuteWithTimeTracking(
                () =>
                {
                    foreach (var item in items)
                    {
                        try
                        {
                            switch (period)
                            {
                                case "MONTHLY":
                                    LoadRepeatPurchaseBehaviorData<VendorRepeatPurchaseBehaviorMonthlyProduct>(item);
                                    break;
                                case "QUATERLY":
                                    LoadRepeatPurchaseBehaviorData<VendorRepeatPurchaseBehaviorQuaterlyProduct>(item);
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Error(
                                extAccount.Id,
                                new Exception($"Could not load data for period {period}. Details: {e}", e));
                        }
                    }
                },
                extAccount.Id,
                "VCD Data Loader",
                AmazonJobOperations.LoadSummaryItemsData);
            return items.Count;
        }

        private Func<T, bool> GetEntityMappingPredicate<T>(
            RepeatPurchaseBehaviorProduct reportEntity, ExtAccount extAccount)
            where T : VendorRepeatPurchaseBehaviorProduct
        {
            return dbEntity => dbEntity.Name == reportEntity.Name && dbEntity.AccountId == extAccount.Id;
        }

        private VendorRepeatPurchaseBehaviorMonthlyProduct MapReportEntityToDbEntityMonthly(
            RepeatPurchaseBehaviorProduct reportEntity, ExtAccount extAccount, DateTime startDate, DateTime endDate)
        {
            var vendorProduct = new VendorRepeatPurchaseBehaviorMonthlyProduct
            {
                Asin = reportEntity.Asin,
                AccountId = extAccount.Id,
                Name = reportEntity.Name,
                UniqueCustomers = reportEntity.UniqueCustomers,
                RepeatPurchaseRevenue = reportEntity.RepeatPurchaseRevenue,
                RepeatPurchaseRevenuePriorPeriod = reportEntity.RepeatPurchaseRevenuePriorPeriod,
                StartDate = startDate,
                EndDate = endDate,
            };
            return vendorProduct;
        }

        private VendorRepeatPurchaseBehaviorQuaterlyProduct MapReportEntityToDbEntityQuaterly(
           RepeatPurchaseBehaviorProduct reportEntity, ExtAccount extAccount, DateTime startDate, DateTime endDate)
        {
            var vendorProduct = new VendorRepeatPurchaseBehaviorQuaterlyProduct
            {
                Asin = reportEntity.Asin,
                AccountId = extAccount.Id,
                Name = reportEntity.Name,
                UniqueCustomers = reportEntity.UniqueCustomers,
                RepeatPurchaseRevenue = reportEntity.RepeatPurchaseRevenue,
                RepeatPurchaseRevenuePriorPeriod = reportEntity.RepeatPurchaseRevenuePriorPeriod,
                StartDate = startDate,
                EndDate = endDate,
            };
            return vendorProduct;
        }
    }
}
