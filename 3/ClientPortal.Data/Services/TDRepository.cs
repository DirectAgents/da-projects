using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs.TD;
using ClientPortal.Data.Entities.TD;
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

        private IQueryable<DailySummary> GetDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID)
        {
            var dailySummaries = context.DailySummaries.AsQueryable();

            if (start.HasValue) dailySummaries = dailySummaries.Where(ds => ds.Date >= start.Value);
            if (end.HasValue) dailySummaries = dailySummaries.Where(ds => ds.Date <= end.Value);

            if (insertionOrderID.HasValue) dailySummaries = dailySummaries.Where(ds => ds.InsertionOrderID == insertionOrderID.Value);

            return dailySummaries;
        }

        public IEnumerable<StatsSummary> GetDailyStatsSummaries(DateTime? start, DateTime? end, int? insertionOrderID, decimal? spendMultiplier = null, decimal? fixedCPM = null, decimal? fixedCPC = null)
        {
            var dailySummaries = GetDailySummaries(start, end, insertionOrderID);
            IQueryable<StatsSummary> statsSummaries;

            if (fixedCPM.HasValue)
            {
                statsSummaries = dailySummaries.Select(ds => new StatsSummary
                {
                    Date = ds.Date,
                    Impressions = ds.Impressions,
                    Clicks = ds.Clicks,
                    Conversions = ds.Conversions,
                    Spend = fixedCPM.Value * ds.Impressions / 1000
                    //Spend = fixedCPM.HasValue ? fixedCPM.Value * ds.Impressions / 1000 : fixedCPC.Value * ds.Clicks
                });
            }
            else if (fixedCPC.HasValue)
            {
                statsSummaries = dailySummaries.Select(ds => new StatsSummary
                {
                    Date = ds.Date,
                    Impressions = ds.Impressions,
                    Clicks = ds.Clicks,
                    Conversions = ds.Conversions,
                    Spend = fixedCPC.Value * ds.Clicks
                });
            }
            else
            {
                decimal spendMult = 1;
                if (spendMultiplier.HasValue)
                    spendMult = spendMultiplier.Value;
                statsSummaries = dailySummaries.Select(ds => new StatsSummary
                {
                    Date = ds.Date,
                    Impressions = ds.Impressions,
                    Clicks = ds.Clicks,
                    Conversions = ds.Conversions,
                    Spend = ds.Revenue * spendMult
                });
            }
            return statsSummaries.ToList();
        }

        private IQueryable<CreativeDailySummary> GetCreativeDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID)
        {
            var cds = context.CreativeDailySummaries.AsQueryable();
            if (start.HasValue)
                cds = cds.Where(c => c.Date >= start.Value);
            if (end.HasValue)
                cds = cds.Where(c => c.Date <= end.Value);
            if (insertionOrderID.HasValue)
                cds = cds.Where(c => c.Creative.InsertionOrderID == insertionOrderID.Value);
            return cds;
        }

        public IEnumerable<CreativeStatsSummary> GetCreativeStatsSummaries(DateTime? start, DateTime? end, int? insertionOrderID, decimal? spendMultiplier = null, decimal? fixedCPM = null, decimal? fixedCPC = null)
        {
            var creativeDailySummaries = GetCreativeDailySummaries(start, end, insertionOrderID);
            IEnumerable<CreativeStatsSummary> creativeSummaries = creativeDailySummaries.GroupBy(cds => cds.Creative).Select(g =>
                new CreativeStatsSummary
                {
                    CreativeID = g.Key.CreativeID,
                    CreativeName = g.Key.CreativeName,
                    Impressions = g.Sum(c => c.Impressions),
                    Clicks = g.Sum(c => c.Clicks),
                    Conversions = g.Sum(c => c.Conversions),
                    Spend = g.Sum(c => c.Revenue)
                }).ToList();

            if (fixedCPM.HasValue || fixedCPC.HasValue)
            {
                creativeSummaries = creativeSummaries.Select(c =>
                    new CreativeStatsSummary
                    {
                        CreativeID = c.CreativeID,
                        CreativeName = c.CreativeName,
                        Impressions = c.Impressions,
                        Clicks = c.Clicks,
                        Conversions = c.Conversions,
                        Spend = fixedCPM.HasValue ? fixedCPM.Value * c.Impressions / 1000 : fixedCPC.Value * c.Clicks
                    });
            }
            else if (spendMultiplier.HasValue)
            {
                creativeSummaries = creativeSummaries.Select(c =>
                    new CreativeStatsSummary
                    {
                        CreativeID = c.CreativeID,
                        CreativeName = c.CreativeName,
                        Impressions = c.Impressions,
                        Clicks = c.Clicks,
                        Conversions = c.Conversions,
                        Spend = c.Spend * spendMultiplier.Value
                    });
            }
            return creativeSummaries;
        }

        // ---

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
