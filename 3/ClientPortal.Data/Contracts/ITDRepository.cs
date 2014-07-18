﻿using ClientPortal.Data.DTOs.TD;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Data.Entities.TD.DBM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClientPortal.Data.Contracts
{
    public interface ITDRepository : IDisposable
    {
        void SaveChanges();

        IEnumerable<StatsSummary> GetDailyStatsSummaries(DateTime? start, DateTime? end, int? insertionOrderID);
        IQueryable<DailySummary> GetDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID);
        IQueryable<CreativeDailySummary> GetCreativeDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID);
        IQueryable<CreativeSummary> GetCreativeSummaries(DateTime? start, DateTime? end, int? insertionOrderID);

        IQueryable<InsertionOrder> InsertionOrders();
        InsertionOrder GetInsertionOrder(int insertionOrderID);

        bool CreateAccountForInsertionOrder(int insertionOrderID);

        IQueryable<TradingDeskAccount> TradingDeskAccounts();
        TradingDeskAccount GetTradingDeskAccount(int tradingDeskAccountId);
    }
}
