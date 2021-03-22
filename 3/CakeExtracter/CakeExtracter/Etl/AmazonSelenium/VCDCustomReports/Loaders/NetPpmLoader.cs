using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.NetPpm;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Loaders
{
    /// <summary>
    /// Loader for NetPpm statistics.
    /// </summary>
    internal class NetPpmLoader : Loader<NetPpmReportData>
    {
        private readonly string period;
        private readonly ExtAccount extAccount;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetPpmLoader"/> class.
        /// </summary>
        /// <param name="extAccount">External account</param>
        /// <param name="period">Period for which data is loaded</param>
        public NetPpmLoader(ExtAccount extAccount, string period)
        {
            this.period = period;
            this.extAccount = extAccount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetPpmLoader"/> class.
        /// </summary>
        /// <returns>List of database products.</returns>
        public List<T> LoadNetPpmData<T>(NetPpmReportData reportData)
            where T : VendorNetPpmProduct
        {
            var dbProducts = EnsureVendorEntitiesInDataBase<T>(reportData.Products, extAccount, reportData.StartDate, reportData.EndDate);
            Logger.Info(extAccount.Id, "Amazon VCD, Finished loading NetPpm data. Loaded metrics of {0} products", dbProducts.Count);
            return dbProducts;
        }

        public List<T> EnsureVendorEntitiesInDataBase<T>(
            List<NetPpmProduct> reportEntities, ExtAccount account, DateTime startDate, DateTime endDate)
            where T : VendorNetPpmProduct
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
                            case "WEEKLY":
                                correspondingDbEntity = MapReportEntityToDbEntityWeekly(reportEntity, account, startDate, endDate) as T;
                                break;
                            case "MONTHLY":
                                correspondingDbEntity = MapReportEntityToDbEntityMonthly(reportEntity, account, startDate, endDate) as T;
                                break;
                            case "YEARLY":
                                correspondingDbEntity = MapReportEntityToDbEntityYearly(reportEntity, account, startDate, endDate) as T;
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

        /// <summary>
        /// Loads Amazon Vendor Central statistics.
        /// </summary>
        /// <param name="items">List of Amazon Vendor Central stats.</param>
        /// <returns>Count of loading items.</returns>
        protected override int Load(List<NetPpmReportData> items)
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
                                case "WEEKLY":
                                    LoadNetPpmData<VendorNetPpmWeeklyProduct>(item);
                                    break;
                                case "MONTHLY":
                                    LoadNetPpmData<VendorNetPpmMonthlyProduct>(item);
                                    break;
                                case "YEARLY":
                                    LoadNetPpmData<VendorNetPpmYearlyProduct>(item);
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
            NetPpmProduct reportEntity, ExtAccount extAccount) where T: VendorNetPpmProduct
        {
            return dbEntity => dbEntity.Name == reportEntity.Name && dbEntity.AccountId == extAccount.Id;
        }

        private VendorNetPpmWeeklyProduct MapReportEntityToDbEntityWeekly(
            NetPpmProduct reportEntity, ExtAccount extAccount, DateTime startDate, DateTime endDate)
        {
            var vendorProduct = new VendorNetPpmWeeklyProduct
            {
                Asin = reportEntity.Asin,
                AccountId = extAccount.Id,
                Name = reportEntity.Name,
                NetPpm = reportEntity.NetPpm,
                NetPpmPriorYearPercentChange = reportEntity.NetPpmPriorYearPercentChange,
                Subcategory = reportEntity.Subcategory,
                StartDate = startDate,
                EndDate = endDate,
            };
            return vendorProduct;
        }

        private VendorNetPpmMonthlyProduct MapReportEntityToDbEntityMonthly(
            NetPpmProduct reportEntity, ExtAccount extAccount, DateTime startDate, DateTime endDate)
        {
            var vendorProduct = new VendorNetPpmMonthlyProduct
            {
                Asin = reportEntity.Asin,
                AccountId = extAccount.Id,
                Name = reportEntity.Name,
                NetPpm = reportEntity.NetPpm,
                NetPpmPriorYearPercentChange = reportEntity.NetPpmPriorYearPercentChange,
                Subcategory = reportEntity.Subcategory,
                StartDate = startDate,
                EndDate = endDate,
            };
            return vendorProduct;
        }

        private VendorNetPpmYearlyProduct MapReportEntityToDbEntityYearly(
            NetPpmProduct reportEntity, ExtAccount extAccount, DateTime startDate, DateTime endDate)
        {
            var vendorProduct = new VendorNetPpmYearlyProduct
            {
                Asin = reportEntity.Asin,
                AccountId = extAccount.Id,
                Name = reportEntity.Name,
                NetPpm = reportEntity.NetPpm,
                NetPpmPriorYearPercentChange = reportEntity.NetPpmPriorYearPercentChange,
                Subcategory = reportEntity.Subcategory,
                StartDate = startDate,
                EndDate = endDate,
            };
            return vendorProduct;
        }
    }
}
