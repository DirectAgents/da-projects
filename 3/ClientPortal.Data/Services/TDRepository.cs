﻿using ClientPortal.Data.Contracts;
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

        public IEnumerable<StatsSummary> GetDailyStatsSummaries(DateTime? start, DateTime? end, int? insertionOrderID)
        {
            var dailySummaries = GetDailySummaries(start, end, insertionOrderID);
            var statsSummaries = dailySummaries.Select(ds => new StatsSummary
            {
                Date = ds.Date,
                Impressions = ds.Impressions,
                Clicks = ds.Clicks,
                Conversions = ds.Conversions,
                Spend = ds.Revenue
            });
            return statsSummaries.ToList();
        }

        public IQueryable<DailySummary> GetDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID)
        {
            var dailySummaries = context.DailySummaries.AsQueryable();

            if (start.HasValue) dailySummaries = dailySummaries.Where(ds => ds.Date >= start.Value);
            if (end.HasValue) dailySummaries = dailySummaries.Where(ds => ds.Date <= end.Value);

            if (insertionOrderID.HasValue) dailySummaries = dailySummaries.Where(ds => ds.InsertionOrderID == insertionOrderID.Value);

            return dailySummaries;
        }

        public IQueryable<CreativeDailySummary> GetCreativeDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID)
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

        public IQueryable<CreativeSummary> GetCreativeSummaries(DateTime? start, DateTime? end, int? insertionOrderID)
        {
            var creativeDailySummaries = GetCreativeDailySummaries(start, end, insertionOrderID);
            var creativeSummaries = creativeDailySummaries.GroupBy(cds => cds.Creative).Select(g =>
                new CreativeSummary {
                    CreativeID = g.Key.CreativeID,
                    CreativeName = g.Key.CreativeName,
                    Impressions = g.Sum(c => c.Impressions),
                    Clicks = g.Sum(c => c.Clicks),
                    Conversions = g.Sum(c => c.Conversions),
                    Spend = g.Sum(c => c.Revenue)
                });

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
