using System.Collections.Generic;
using CakeExtracter.Etl.TradingDesk.LoadersDA.Adders;
using CakeExtracter.Etl.TradingDesk.LoadersDA.Interfaces;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    public class AmazonPdaDailySummaryLoader : Loader<DailySummary>
    {
        private readonly IDailySummaryLoader tdDailySummaryLoader;

        public AmazonPdaDailySummaryLoader(int accountId) : base(accountId)
        {
            tdDailySummaryLoader = new DailySummaryAdder(accountId);
        }

        protected override int Load(List<DailySummary> items)
        {
            Logger.Info(accountId, "Loading {0} AmazonDailySummaries (to add)...", items.Count);
            tdDailySummaryLoader.AssignIdsToItems(items);
            var count = tdDailySummaryLoader.UpsertDailySummaries(items);
            return count;
        }
    }
}