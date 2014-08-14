using ClientPortal.Data.DTOs.TD;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Data.Entities.TD.AdRoll;
using ClientPortal.Data.Entities.TD.DBM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClientPortal.Data.Contracts
{
    public interface ITDRepository : IDisposable
    {
        void SaveChanges();

        IEnumerable<StatsSummary> GetDailyStatsSummaries(DateTime? start, DateTime? end, TradingDeskAccount tdAccount);
        IEnumerable<CreativeStatsSummary> GetCreativeStatsSummaries(DateTime? start, DateTime? end, TradingDeskAccount tdAccount);

        IQueryable<AdRollProfile> AdRollProfiles();

        IQueryable<InsertionOrder> InsertionOrders();
        InsertionOrder GetInsertionOrder(int insertionOrderID);

        bool CreateAccountForInsertionOrder(int insertionOrderID);

        IQueryable<TradingDeskAccount> TradingDeskAccounts();
        TradingDeskAccount GetTradingDeskAccount(int tradingDeskAccountId);
        void SaveTradingDeskAccount(TradingDeskAccount tdAccount);
    }
}
