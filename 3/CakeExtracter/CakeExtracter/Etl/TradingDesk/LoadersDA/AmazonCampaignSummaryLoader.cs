using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class AmazonCampaignSummaryLoader : Loader<StrategySummary>
    {
        private readonly TDStrategySummaryLoader tdStrategySummaryLoader;

        public AmazonCampaignSummaryLoader(int accountId)
            : base(accountId)
        {
            tdStrategySummaryLoader = new TDStrategySummaryLoader(accountId);
        }

        protected override int Load(List<StrategySummary> items)
        {
            Logger.Info(accountId, "Loading {0} Amazon Campaign Daily Summaries..", items.Count);

            tdStrategySummaryLoader.PrepareData(items);
            tdStrategySummaryLoader.AddUpdateDependentStrategies(items);
            tdStrategySummaryLoader.AssignStrategyIdToItems(items);
            var count = tdStrategySummaryLoader.UpsertDailySummaries(items);

            return count;
        }
    }
}
