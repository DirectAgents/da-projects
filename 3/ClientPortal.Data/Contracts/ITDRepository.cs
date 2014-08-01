using ClientPortal.Data.DTOs.TD;
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

        //IQueryable<DailySummary> GetDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID);
        IEnumerable<StatsSummary> GetDailyStatsSummaries(DateTime? start, DateTime? end, int? insertionOrderID, decimal? spendMultiplier = null, decimal? fixedCPM = null, decimal? fixedCPC = null);
        //IQueryable<CreativeDailySummary> GetCreativeDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID);
        IQueryable<CreativeStatsSummary> GetCreativeStatsSummaries(DateTime? start, DateTime? end, int? insertionOrderID);

        IQueryable<InsertionOrder> InsertionOrders();
        InsertionOrder GetInsertionOrder(int insertionOrderID);

        bool CreateAccountForInsertionOrder(int insertionOrderID);

        IQueryable<TradingDeskAccount> TradingDeskAccounts();
        TradingDeskAccount GetTradingDeskAccount(int tradingDeskAccountId);
        void SaveTradingDeskAccount(TradingDeskAccount tdAccount);
    }
}
