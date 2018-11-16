using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
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
