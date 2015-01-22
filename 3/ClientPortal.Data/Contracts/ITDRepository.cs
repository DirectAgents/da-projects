using System;
using System.Collections.Generic;
using System.Linq;
using ClientPortal.Data.DTOs.TD;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Data.Entities.TD.AdRoll;
using ClientPortal.Data.Entities.TD.DBM;

namespace ClientPortal.Data.Contracts
{
    public interface ITDRepository : IDisposable
    {
        void SaveChanges();

        IEnumerable<StatsSummary> GetDailyStatsSummaries(DateTime? start, DateTime? end, TradingDeskAccount tdAccount);
        IEnumerable<CreativeStatsSummary> GetCreativeStatsSummaries(DateTime? start, DateTime? end, TradingDeskAccount tdAccount);

        StatsRollup AdRollStatsRollup(int profileId);
        IQueryable<AdRollProfile> AdRollProfiles();

        IQueryable<InsertionOrder> InsertionOrders();
        InsertionOrder GetInsertionOrder(int insertionOrderID);

        bool CreateAccountForInsertionOrder(int insertionOrderID);

        IQueryable<TradingDeskAccount> TradingDeskAccounts();
        TradingDeskAccount GetTradingDeskAccount(int tradingDeskAccountId);
        void SaveTradingDeskAccount(TradingDeskAccount tdAccount);
    }
}
