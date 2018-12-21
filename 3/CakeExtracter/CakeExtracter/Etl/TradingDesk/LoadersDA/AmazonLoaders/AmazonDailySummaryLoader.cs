using System.Collections.Generic;
using CakeExtracter.Etl.TradingDesk.LoadersDA.Adders;
using CakeExtracter.Etl.TradingDesk.LoadersDA.Interfaces;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    public class AmazonDailySummaryLoader : Loader<DailySummary>
    {
        private readonly TDDailySummaryLoader tdDailySummaryLoader;

        public AmazonDailySummaryLoader(int accountId) : base(accountId)
        {
            tdDailySummaryLoader = new TDDailySummaryLoader(accountId);
        }

        protected override int Load(List<DailySummary> items)
        {
            Logger.Info(accountId, "Loading {0} AmazonDailySummaries..", items.Count);
            tdDailySummaryLoader.AssignIdsToItems(items);
            var count = tdDailySummaryLoader.UpsertDailySummaries(items);
            return count;
        }
    }
}
