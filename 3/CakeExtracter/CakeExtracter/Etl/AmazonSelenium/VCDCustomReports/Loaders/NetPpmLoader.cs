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
using DirectAgents.Domain.Entities.CPProg.Vendor.NetPpm;
using SeleniumDataBrowser.VCD.Enums;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Loaders
{
    class NetPpmLoader : VcdCustomReportLoader<NetPpmProduct, VendorNetPpmProduct>
    {
        private readonly PeriodType period;

        private readonly Dictionary<PeriodType, Func<NetPpmProduct, DateRange, ExtAccount, VendorNetPpmProduct>> periodProductTypeDictionary
            = new Dictionary<PeriodType, Func<NetPpmProduct, DateRange, ExtAccount, VendorNetPpmProduct>>
        {
            { PeriodType.WEEKLY, CreateWeekly },
            { PeriodType.MONTHLY, CreateMonthly },
            { PeriodType.YEARLY, CreateYearly },
        };

        public NetPpmLoader(ExtAccount extAccount, PeriodType period)
            : base(extAccount)
        {
            this.period = period;
        }

        protected override VendorNetPpmProduct MapReportEntity(NetPpmProduct reportProduct, DateRange dateRange)
        {
            return periodProductTypeDictionary[period](reportProduct, dateRange, extAccount);
        }

        protected override void RemoveOldData(VcdCustomReportData<NetPpmProduct> item)
        {
            Logger.Info(
                accountId,
                $"The cleaning of NetPpm for account ({accountId}) has begun - from {item.ReportDateRange.FromDate} to {item.ReportDateRange.ToDate}.");
            SafeContextWrapper.TryMakeTransaction(
                (ClientPortalProgContext db) =>
                {
                    if (period == PeriodType.WEEKLY)
                    {
                        db.VendorNetPpmWeeklyProducts.Where(x =>
                        x.StartDate >= item.ReportDateRange.FromDate &&
                        x.EndDate <= item.ReportDateRange.ToDate &&
                        x.AccountId == accountId).DeleteFromQuery();
                    }

                    if (period == PeriodType.MONTHLY)
                    {
                        db.VendorNetPpmMonthlyProducts.Where(x =>
                        x.StartDate >= item.ReportDateRange.FromDate &&
                        x.EndDate <= item.ReportDateRange.ToDate &&
                        x.AccountId == accountId).DeleteFromQuery();
                    }

                    if (period == PeriodType.YEARLY)
                    {
                        db.VendorNetPpmYearlyProducts.Where(x =>
                        x.StartDate >= item.ReportDateRange.FromDate &&
                        x.EndDate <= item.ReportDateRange.ToDate &&
                        x.AccountId == accountId).DeleteFromQuery();
                    }
                }, "DeleteFromQuery");
            Logger.Info(
                accountId,
                $"The cleaning of NetPpm data for account ({accountId}) is over - from {item.ReportDateRange.FromDate} to {item.ReportDateRange.ToDate}.");
        }

        private static VendorNetPpmWeeklyProduct CreateWeekly(NetPpmProduct product, DateRange dateRange, ExtAccount account)
        {
            return new VendorNetPpmWeeklyProduct()
            {
                AccountId = account.Id,
                Asin = product.Asin,
                Subcategory = product.Subcategory,
                Name = product.Name,
                NetPpm = product.NetPpm,
                NetPpmPriorYearPercentChange = product.NetPpmPriorYearPercentChange,
                StartDate = dateRange.FromDate,
                EndDate = dateRange.ToDate,
            };
        }

        private static VendorNetPpmMonthlyProduct CreateMonthly(NetPpmProduct product, DateRange dateRange, ExtAccount account)
        {
            return new VendorNetPpmMonthlyProduct()
            {
                AccountId = account.Id,
                Asin = product.Asin,
                Subcategory = product.Subcategory,
                Name = product.Name,
                NetPpm = product.NetPpm,
                NetPpmPriorYearPercentChange = product.NetPpmPriorYearPercentChange,
                StartDate = dateRange.FromDate,
                EndDate = dateRange.ToDate,
            };
        }

        private static VendorNetPpmYearlyProduct CreateYearly(NetPpmProduct product, DateRange dateRange, ExtAccount account)
        {
            return new VendorNetPpmYearlyProduct()
            {
                AccountId = account.Id,
                Asin = product.Asin,
                Subcategory = product.Subcategory,
                Name = product.Name,
                NetPpm = product.NetPpm,
                NetPpmPriorYearPercentChange = product.NetPpmPriorYearPercentChange,
                StartDate = dateRange.FromDate,
                EndDate = dateRange.ToDate,
            };
        }
    }
}
