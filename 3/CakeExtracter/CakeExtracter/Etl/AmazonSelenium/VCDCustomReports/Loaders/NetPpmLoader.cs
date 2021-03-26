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
    /// <summary>
    /// Loader for Net PPM statistics.
    /// </summary>
    class NetPpmLoader : VcdCustomReportLoader<NetPpmProduct, VendorNetPpmProduct>
    {
        private readonly PeriodType period;

        private readonly Dictionary<PeriodType, Func<VendorNetPpmProduct>> periodProductTypeDictionary
            = new Dictionary<PeriodType, Func<VendorNetPpmProduct>>
        {
            { PeriodType.WEEKLY, CreateWeekly },
            { PeriodType.MONTHLY, CreateMonthly },
            { PeriodType.YEARLY, CreateYearly },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="NetPpmLoader"/> class.
        /// </summary>
        /// <param name="extAccount"></param>
        /// <param name="period"></param>
        public NetPpmLoader(ExtAccount extAccount, PeriodType period)
            : base(extAccount)
        {
            this.period = period;
        }

        protected override VendorNetPpmProduct MapReportEntity(NetPpmProduct reportProduct, DateRange dateRange)
        {
            var reportEntity = periodProductTypeDictionary[period]();
            reportEntity.AccountId = extAccount.Id;
            reportEntity.Asin = reportProduct.Asin;
            reportEntity.Subcategory = reportProduct.Subcategory;
            reportEntity.Name = reportProduct.Name;
            reportEntity.NetPpm = reportProduct.NetPpm;
            reportEntity.NetPpmPriorYearPercentChange = reportProduct.NetPpmPriorYearPercentChange;
            reportEntity.StartDate = dateRange.FromDate;
            reportEntity.EndDate = dateRange.ToDate;
            return reportEntity;
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

        protected override void BulkSaveData(VcdCustomReportData<NetPpmProduct> item)
        {
            using (var dbContext = new ClientPortalProgContext())
            {
                var dateRange = item.ReportDateRange;
                var products = item.Products.Select(x => MapReportEntity(x, dateRange));
                dbContext.BulkInsert(products);
            }
        }

        private static VendorNetPpmWeeklyProduct CreateWeekly()
        {
            return new VendorNetPpmWeeklyProduct();
        }

        private static VendorNetPpmMonthlyProduct CreateMonthly()
        {
            return new VendorNetPpmMonthlyProduct();
        }

        private static VendorNetPpmYearlyProduct CreateYearly()
        {
            return new VendorNetPpmYearlyProduct();
        }
    }
}
