using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs.TD;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Data.Entities.TD.AdRoll;
using ClientPortal.Data.Entities.TD.DBM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ClientPortal.Data.Services
{
    public class TDRepository : ITDRepository, IDisposable
    {
        private TDContext context;

        public TDRepository(TDContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        // ---

        private IQueryable<DBMDailySummary> GetDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID)
        {
            var dailySummaries = context.DBMDailySummaries.AsQueryable();

            if (start.HasValue) dailySummaries = dailySummaries.Where(ds => ds.Date >= start.Value);
            if (end.HasValue) dailySummaries = dailySummaries.Where(ds => ds.Date <= end.Value);

            if (insertionOrderID.HasValue) dailySummaries = dailySummaries.Where(ds => ds.InsertionOrderID == insertionOrderID.Value);

            return dailySummaries;
        }

        private IEnumerable<StatsSummary> GetDailyStatsSummariesDBM(DateTime? start, DateTime? end, int insertionOrderID)
        {
            //var dailySummaries = GetDailySummaries(start, end, insertionOrderID);
            //var statsSummaries = dailySummaries.Select(s => new StatsSummary
            //{
            //    Date = s.Date,
            //    Impressions = s.Impressions,
            //    Clicks = s.Clicks,
            //    Conversions = s.Conversions,
            //    Spend = s.Revenue
            //}).ToList();
            var dailySummaries = GetCreativeDailySummaries(start, end, insertionOrderID);
            var statsSummaries = dailySummaries.GroupBy(cds => cds.Date).Select(g =>
                new StatsSummary
                {
                    Date = g.Key,
                    Impressions = g.Sum(s => s.Impressions),
                    Clicks = g.Sum(s => s.Clicks),
                    Conversions = g.Sum(s => s.Conversions),
                    Spend = g.Sum(s => s.Revenue)
                }).ToList();
            return statsSummaries;
        }

        private IEnumerable<StatsSummary> GetDailyStatsSummariesAdRoll(DateTime? start, DateTime? end, int adrollProfileId)
        {
            var dailySummaries = GetAdDailySummaries(start, end, adrollProfileId);
            var summaries = dailySummaries.GroupBy(ads => ads.Date).Select(g =>
                new StatsSummary
                {
                    Date = g.Key,
                    Impressions = g.Sum(s => s.Impressions),
                    Clicks = g.Sum(s => s.Clicks),
                    Conversions = g.Sum(s => s.Conversions),
                    Spend = g.Sum(s => s.Spend)
                }).ToList();
            return summaries;
        }

        public IEnumerable<StatsSummary> GetDailyStatsSummaries(DateTime? start, DateTime? end, TradingDeskAccount tda)
        {
            int? insertionOrderID = tda.InsertionOrderID();
            int? adrollProfileId = tda.AdRollProfileId();
            if (!insertionOrderID.HasValue && !adrollProfileId.HasValue)
                return new List<StatsSummary>();

            IEnumerable<StatsSummary> summaries = null;
            if (insertionOrderID.HasValue)
                summaries = GetDailyStatsSummariesDBM(start, end, insertionOrderID.Value);
            if (adrollProfileId.HasValue)
            {
                var summariesAdRoll = GetDailyStatsSummariesAdRoll(start, end, adrollProfileId.Value);
                if (!insertionOrderID.HasValue)
                    summaries = summariesAdRoll;
                else
                { // combine two sources
                    summaries = summaries.Concat(summariesAdRoll);
                    summaries = summaries.GroupBy(s => s.Date).Select(g =>
                        new StatsSummary
                        {
                            Date = g.Key,
                            Impressions = g.Sum(s => s.Impressions),
                            Clicks = g.Sum(s => s.Clicks),
                            Conversions = g.Sum(s => s.Conversions),
                            Spend = g.Sum(s => s.Spend)
                        }).ToList();
                }
            }

            if (tda.FixedCPM.HasValue || tda.FixedCPC.HasValue)
            {
                summaries = summaries.Select(s =>
                    new StatsSummary
                    {
                        Date = s.Date,
                        Impressions = s.Impressions,
                        Clicks = s.Clicks,
                        Conversions = s.Conversions,
                        Spend = tda.FixedCPM.HasValue ? tda.FixedCPM.Value * s.Impressions / 1000 : tda.FixedCPC.Value * s.Clicks
                    });
            }
            else if (tda.SpendMultiplier.HasValue)
            {
                summaries = summaries.Select(s =>
                    new StatsSummary
                    {
                        Date = s.Date,
                        Impressions = s.Impressions,
                        Clicks = s.Clicks,
                        Conversions = s.Conversions,
                        Spend = s.Spend * tda.SpendMultiplier.Value
                    });
            }
            return summaries;
        }

        // ---

        private IQueryable<CreativeDailySummary> GetCreativeDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID)
        {
            var cds = context.CreativeDailySummaries.AsQueryable();
            if (start.HasValue)
                cds = cds.Where(s => s.Date >= start.Value);
            if (end.HasValue)
                cds = cds.Where(s => s.Date <= end.Value);
            if (insertionOrderID.HasValue)
                cds = cds.Where(s => s.Creative.InsertionOrderID == insertionOrderID.Value);
            return cds;
        }

        private IEnumerable<CreativeStatsSummary> GetCreativeStatsSummariesDBM(DateTime? start, DateTime? end, int insertionOrderID)
        {
            var creativeDailySummaries = GetCreativeDailySummaries(start, end, insertionOrderID);
            var creativeStatsSummaries = creativeDailySummaries.GroupBy(cds => cds.Creative).Select(g =>
                new CreativeStatsSummary
                {
                    CreativeID = g.Key.CreativeID,
                    CreativeName = g.Key.CreativeName,
                    Impressions = g.Sum(c => c.Impressions),
                    Clicks = g.Sum(c => c.Clicks),
                    Conversions = g.Sum(c => c.Conversions),
                    Spend = g.Sum(c => c.Revenue)
                }).ToList();
            return creativeStatsSummaries;
        }

        private IQueryable<AdDailySummary> GetAdDailySummaries(DateTime? start, DateTime? end, int? adrollProfileId)
        {
            var ads = context.AdDailySummaries.AsQueryable();
            if (start.HasValue)
                ads = ads.Where(s => s.Date >= start.Value);
            if (end.HasValue)
                ads = ads.Where(s => s.Date <= end.Value);
            if (adrollProfileId.HasValue)
                ads = ads.Where(s => s.AdRollAd.AdRollProfileId == adrollProfileId.Value);
            return ads;
        }

        private IEnumerable<CreativeStatsSummary> GetCreativeStatsSummariesAdRoll(DateTime? start, DateTime? end, int adrollProfileId)
        {
            var dailySummaries = GetAdDailySummaries(start, end, adrollProfileId);
            var summaries = dailySummaries.GroupBy(ads => ads.AdRollAd).Select(g =>
                new CreativeStatsSummary
                {
                    CreativeID = g.Key.Id,
                    CreativeName = g.Key.Name,
                    Impressions = g.Sum(s => s.Impressions),
                    Clicks = g.Sum(s => s.Clicks),
                    Conversions = g.Sum(s => s.Conversions),
                    Spend = g.Sum(s => s.Spend)
                }).ToList();
            return summaries;
        }

        public IEnumerable<CreativeStatsSummary> GetCreativeStatsSummaries(DateTime? start, DateTime? end, TradingDeskAccount tda)
        {
            //TODO: find matching creatives from the two sources and combine their stats
            int? insertionOrderID = tda.InsertionOrderID();
            int? adrollProfileId = tda.AdRollProfileId();
            IEnumerable<CreativeStatsSummary> summaries;

            if (insertionOrderID.HasValue)
                summaries = GetCreativeStatsSummariesDBM(start, end, insertionOrderID.Value);
            else
                summaries = new List<CreativeStatsSummary>();
            if (adrollProfileId.HasValue)
                summaries = summaries.Concat(GetCreativeStatsSummariesAdRoll(start, end, adrollProfileId.Value));

            if (tda.FixedCPM.HasValue || tda.FixedCPC.HasValue)
            {
                summaries = summaries.Select(c =>
                    new CreativeStatsSummary
                    {
                        CreativeID = c.CreativeID,
                        CreativeName = c.CreativeName,
                        Impressions = c.Impressions,
                        Clicks = c.Clicks,
                        Conversions = c.Conversions,
                        Spend = tda.FixedCPM.HasValue ? tda.FixedCPM.Value * c.Impressions / 1000 : tda.FixedCPC.Value * c.Clicks
                    });
            }
            else if (tda.SpendMultiplier.HasValue)
            {
                summaries = summaries.Select(c =>
                    new CreativeStatsSummary
                    {
                        CreativeID = c.CreativeID,
                        CreativeName = c.CreativeName,
                        Impressions = c.Impressions,
                        Clicks = c.Clicks,
                        Conversions = c.Conversions,
                        Spend = c.Spend * tda.SpendMultiplier.Value
                    });
            }
            return summaries;
        }

        // ---

        public StatsRollup AdRollStatsRollup(int profileId)
        {
            var adDailySummaries = context.AdDailySummaries.Where(s => s.AdRollAd.AdRollProfileId == profileId);
            var statsRollup = new StatsRollup();
            if (adDailySummaries.Any())
            {
                statsRollup.MinDate = adDailySummaries.Min(s => s.Date);
                statsRollup.MaxDate = adDailySummaries.Max(s => s.Date);
                statsRollup.NumCreatives = adDailySummaries.Select(s => s.AdRollAdId).Distinct().Count();
            }
            return statsRollup;
        }

        // ---

        public IQueryable<AdRollProfile> AdRollProfiles()
        {
            var profiles = context.AdRollProfiles;
            return profiles;
        }

        public IQueryable<InsertionOrder> InsertionOrders()
        {
            var insertionOrders = context.InsertionOrders;
            return insertionOrders;
        }

        public InsertionOrder GetInsertionOrder(int insertionOrderID)
        {
            var insertionOrder = context.InsertionOrders.Find(insertionOrderID);
            return insertionOrder;
        }

        public bool CreateAccountForInsertionOrder(int insertionOrderID)
        {
            var insertionOrder = context.InsertionOrders.Find(insertionOrderID);
            if (insertionOrder == null)
                return false;

            var tdAccount = NewTradingDeskAccount();
            insertionOrder.TradingDeskAccount = tdAccount;
            context.SaveChanges();
            return true;
        }

        private TradingDeskAccount NewTradingDeskAccount()
        {
            return new TradingDeskAccount();
        }

        public IQueryable<TradingDeskAccount> TradingDeskAccounts()
        {
            var tdAccounts = context.TradingDeskAccounts;
            return tdAccounts;
        }

        public TradingDeskAccount GetTradingDeskAccount(int tradingDeskAccountId)
        {
            var tdAccount = context.TradingDeskAccounts.Find(tradingDeskAccountId);
            return tdAccount;
        }

        public void SaveTradingDeskAccount(TradingDeskAccount tdAccount)
        {
            var entry = context.Entry(tdAccount);
            entry.State = EntityState.Modified;
            context.SaveChanges();
        }

        // ---

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
