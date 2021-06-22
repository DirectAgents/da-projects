using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Vendor;
using DirectAgents.Domain.Entities.CPProg.Vendor.NetPpm;
using DirectAgents.Domain.SpecialPlatformProviders.Contracts;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.SpecialPlatformProviders.Implementation
{
    public class VcdProvider : SpecialPlatformProvider
    {
        public VcdProvider() : base(Platform.Code_AraAmazon)
        {
        }

        public override IEnumerable<SpecialPlatformLatestsSummary> GetDatesRangeByAccounts(ClientPortalProgContext context)
        {
            var vcdSummaries = GetSummariesGroupedByAccountId(context).ToList();
            AssignExtAccountForSummaries(vcdSummaries, context);
            return vcdSummaries;
        }

        public IEnumerable<CustomSpecialPlatformLatestsSummary> GetCustomDatesRangeByAccounts(ClientPortalProgContext context)
        {
            var customSummaries = GetCustomSummariesGroupedByAccountId(context).ToList();
            AssignExtAccountForSummaries(customSummaries, context);
            return customSummaries;
        }

        private IEnumerable<SpecialPlatformLatestsSummary> GetSummariesGroupedByAccountId(
            ClientPortalProgContext context)
        {
            return context.VcdAnalytic
                    .GroupBy(x => x.AccountId)
                    .Select(x => new SpecialPlatformLatestsSummary
                    {
                        AccountId = x.Key,
                        EarliestDate = x.Min(z => z.Date),
                        LatestDate = x.Max(z => z.Date),
                    });
        }

        private IEnumerable<CustomSpecialPlatformLatestsSummary> GetCustomSummariesGroupedByAccountId(
            ClientPortalProgContext context)
        {
            var netPpmWeekly = GetTwoDatesSummariesGroupedByAccountId(context.VendorNetPpmWeeklyProducts).ToList();
            var netPpmMonthly = GetTwoDatesSummariesGroupedByAccountId(context.VendorNetPpmMonthlyProducts).ToList();
            var netPpmYearly = GetTwoDatesSummariesGroupedByAccountId(context.VendorNetPpmYearlyProducts).ToList();
            var repeatPurchaseMonthly = GetTwoDatesSummariesGroupedByAccountId(context.VendorRepeatPurchaseBehaviorMonthlyProducts).ToList();
            var repeatPurchaseQuarterly = GetTwoDatesSummariesGroupedByAccountId(context.VendorRepeatPurchaseBehaviorQuarterlyProducts).ToList();

            var geoSales = GetOneDateSummariesGroupedByAccountId(context.VendorGeographicSalesInsightsProducts).ToList();
            var itemComp = GetOneDateSummariesGroupedByAccountId(context.VendorItemComparisonProductMetrics).ToList();
            var altPurchase = GetOneDateSummariesGroupedByAccountId(context.VendorAlternativePurchaseProductMetrics).ToList();
            var marketBasket = GetOneDateSummariesGroupedByAccountId(context.VendorMarketBasketAnalysisProductMetrics).ToList();

            var summaries = from nw in netPpmWeekly
                            join nm in netPpmMonthly on nw.AccountId equals nm.AccountId
                            join ny in netPpmYearly on nw.AccountId equals ny.AccountId
                            join rm in repeatPurchaseMonthly on nw.AccountId equals rm.AccountId
                            join rq in repeatPurchaseQuarterly on nw.AccountId equals rq.AccountId
                            join gs in geoSales on nw.AccountId equals gs.AccountId
                            join ic in itemComp on nw.AccountId equals ic.AccountId
                            join ap in altPurchase on nw.AccountId equals ap.AccountId
                            join mb in marketBasket on nw.AccountId equals mb.AccountId
                            select new CustomSpecialPlatformLatestsSummary
                            {
                                AccountId = nw.AccountId,
                                EarliestDateNetPpmWeekly = nw.EarliestDate,
                                LatestDateNetPpmWeekly = nw.LatestDate,
                                EarliestDateNetPpmMonthly = nm.EarliestDate,
                                LatestDateNetPpmMonthly = nm.LatestDate,
                                EarliestDateNetPpmYearly = ny.EarliestDate,
                                LatestDateNetPpmYearly = ny.LatestDate,
                                EarliestDateRepeatPurchaseMonthly = rm.EarliestDate,
                                LatestDateRepeatPurchaseMonthly = rm.LatestDate,
                                EarliestDateRepeatPurchaseQuarterly = rq.EarliestDate,
                                LatestDateRepeatPurchaseQuarterly = rq.LatestDate,
                                EarliestDateGeoSales = gs.EarliestDate,
                                LatestDateGeoSales = gs.LatestDate,
                                EarliestDateItemComp = ic.EarliestDate,
                                LatestDateItemComp = ic.LatestDate,
                                EarliestDateAltPurchase = ap.EarliestDate,
                                LatestDateAltPurchase = ap.LatestDate,
                                EarliestDateMarketBasket = mb.EarliestDate,
                                LatestDateMarketBasket = mb.LatestDate,
                            };
            return summaries;
        }

        private IQueryable<SpecialPlatformLatestsSummary> GetOneDateSummariesGroupedByAccountId<T>(
            DbSet<T> products)
            where T : BaseVendorEntity, IVendorProductDate
        {
            return products
                .GroupBy(x => x.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date),
                });
        }

        private IQueryable<SpecialPlatformLatestsSummary> GetTwoDatesSummariesGroupedByAccountId<T>(
            DbSet<T> products)
            where T : BaseVendorEntity, IVendorProductDateRange
        {
            return products
                .GroupBy(x => x.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.StartDate),
                    LatestDate = x.Max(z => z.EndDate),
                });
        }
    }
}
