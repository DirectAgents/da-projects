using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Products;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.RepeatPurchaseBehavior;
using SeleniumDataBrowser.VCD.Enums;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Loaders
{
    /// <summary>
    /// Loader for Repeat Purchase Behavior statistics.
    /// </summary>
    public class RepeatPurchaseBehaviorLoader : VcdCustomReportLoader<RepeatPurchaseBehaviorProduct, VendorRepeatPurchaseBehaviorProduct>
    {
        private readonly PeriodType period;

        private readonly Dictionary<PeriodType, Func<RepeatPurchaseBehaviorProduct, DateRange, ExtAccount,
            VendorRepeatPurchaseBehaviorProduct>> periodProductTypeDictionary
            = new Dictionary<PeriodType, Func<RepeatPurchaseBehaviorProduct, DateRange, ExtAccount, VendorRepeatPurchaseBehaviorProduct>>
        {
            { PeriodType.MONTHLY, CreateMonthly },
            { PeriodType.QUARTERLY, CreateQuarterly },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatPurchaseBehaviorLoader"/> class.
        /// </summary>
        /// <param name="extAccount"></param>
        /// <param name="period"></param>
        public RepeatPurchaseBehaviorLoader(ExtAccount extAccount, PeriodType period)
            : base(extAccount)
        {
            this.period = period;
        }

        /// <inheritdoc/>
        protected override VendorRepeatPurchaseBehaviorProduct MapReportEntity(RepeatPurchaseBehaviorProduct reportProduct, DateRange dateRange)
        {
            var reportEntity = periodProductTypeDictionary[period](reportProduct, dateRange, extAccount);
            reportEntity.AccountId = extAccount.Id;
            reportEntity.Asin = reportProduct.Asin;
            reportEntity.Name = reportProduct.Name;
            reportEntity.UniqueCustomers = reportProduct.UniqueCustomers;
            reportEntity.RepeatPurchaseRevenue = reportProduct.RepeatPurchaseRevenue;
            reportEntity.RepeatPurchaseRevenuePriorPeriod = reportProduct.RepeatPurchaseRevenuePriorPeriod;
            reportEntity.StartDate = dateRange.FromDate;
            reportEntity.EndDate = dateRange.ToDate;
            return reportEntity;
        }

        /// <inheritdoc/>
        protected override void RemoveOldData(VcdCustomReportData<RepeatPurchaseBehaviorProduct> item)
        {
            Logger.Info(
                accountId,
                $"The cleaning of RepeatPurchaseBehavior data for account ({accountId}) has begun - from {item.ReportDateRange.FromDate} to {item.ReportDateRange.ToDate}.");
            SafeContextWrapper.TryMakeTransaction(
                (ClientPortalProgContext db) =>
                {
                    if (period == PeriodType.MONTHLY)
                    {
                        db.VendorRepeatPurchaseBehaviorMonthlyProducts.Where(x =>
                        x.StartDate >= item.ReportDateRange.FromDate &&
                        x.EndDate <= item.ReportDateRange.ToDate &&
                        x.AccountId == accountId).DeleteFromQuery();
                    }

                    if (period == PeriodType.QUARTERLY)
                    {
                        db.VendorRepeatPurchaseBehaviorQuarterlyProducts.Where(x =>
                        x.StartDate >= item.ReportDateRange.FromDate &&
                        x.EndDate <= item.ReportDateRange.ToDate &&
                        x.AccountId == accountId).DeleteFromQuery();
                    }
                }, "DeleteFromQuery");
            Logger.Info(
                accountId,
                $"The cleaning of RepeatPurchaseBehavior data for account ({accountId}) is over - from {item.ReportDateRange.FromDate} to {item.ReportDateRange.ToDate}.");
        }

        /// <inheritdoc/>
        protected override void BulkSaveData(VcdCustomReportData<RepeatPurchaseBehaviorProduct> item)
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                var dateRange = item.ReportDateRange;
                var products = item.Products.Select(x => MapReportEntity(x, dateRange));
                dbContext.BulkInsert(products);
            }
        }

        private static VendorRepeatPurchaseBehaviorMonthlyProduct CreateMonthly(RepeatPurchaseBehaviorProduct product, DateRange dateRange, ExtAccount account)
        {
            return new VendorRepeatPurchaseBehaviorMonthlyProduct();
        }

        private static VendorRepeatPurchaseBehaviorQuarterlyProduct CreateQuarterly(RepeatPurchaseBehaviorProduct product, DateRange dateRange, ExtAccount account)
        {
            return new VendorRepeatPurchaseBehaviorQuarterlyProduct();
        }
    }
}
