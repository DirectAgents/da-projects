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

        public RepeatPurchaseBehaviorLoader(ExtAccount extAccount, PeriodType period)
            : base(extAccount)
        {
            this.period = period;
        }

        protected override VendorRepeatPurchaseBehaviorProduct MapReportEntity(RepeatPurchaseBehaviorProduct reportProduct, DateRange dateRange)
        {
            return periodProductTypeDictionary[period](reportProduct, dateRange, extAccount);
        }

        protected override void RemoveOldData(VcdCustomReportData<RepeatPurchaseBehaviorProduct> item)
        {
            Logger.Info(
                accountId,
                $"The cleaning of NetPpm for account ({accountId}) has begun - from {item.ReportDateRange.FromDate} to {item.ReportDateRange.ToDate}.");
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
                        db.VendorRepeatPurchaseBehaviorQuaterlyProducts.Where(x =>
                        x.StartDate >= item.ReportDateRange.FromDate &&
                        x.EndDate <= item.ReportDateRange.ToDate &&
                        x.AccountId == accountId).DeleteFromQuery();
                    }
                }, "DeleteFromQuery");
            Logger.Info(
                accountId,
                $"The cleaning of NetPpm data for account ({accountId}) is over - from {item.ReportDateRange.FromDate} to {item.ReportDateRange.ToDate}.");
        }

        private static VendorRepeatPurchaseBehaviorMonthlyProduct CreateMonthly(RepeatPurchaseBehaviorProduct product, DateRange dateRange, ExtAccount account)
        {
            return new VendorRepeatPurchaseBehaviorMonthlyProduct()
            {
                AccountId = account.Id,
                Asin = product.Asin,
                Name = product.Name,
                UniqueCustomers = product.UniqueCustomers,
                RepeatPurchaseRevenue = product.RepeatPurchaseRevenue,
                RepeatPurchaseRevenuePriorPeriod = product.RepeatPurchaseRevenuePriorPeriod,
                StartDate = dateRange.FromDate,
                EndDate = dateRange.ToDate,
            };
        }

        private static VendorRepeatPurchaseBehaviorQuarterlyProduct CreateQuarterly(RepeatPurchaseBehaviorProduct product, DateRange dateRange, ExtAccount account)
        {
            return new VendorRepeatPurchaseBehaviorQuarterlyProduct()
            {
                AccountId = account.Id,
                Asin = product.Asin,
                Name = product.Name,
                UniqueCustomers = product.UniqueCustomers,
                RepeatPurchaseRevenue = product.RepeatPurchaseRevenue,
                RepeatPurchaseRevenuePriorPeriod = product.RepeatPurchaseRevenuePriorPeriod,
                StartDate = dateRange.FromDate,
                EndDate = dateRange.ToDate,
            };
        }
    }
}
