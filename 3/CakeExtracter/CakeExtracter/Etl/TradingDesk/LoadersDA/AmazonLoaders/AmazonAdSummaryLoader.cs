using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    public class AmazonAdSummaryLoader : Loader<TDadSummary>
    {
        private readonly TDadSummaryLoader tdAdSummaryLoader;

        public AmazonAdSummaryLoader(int accountId)
        {
            this.tdAdSummaryLoader = new TDadSummaryLoader(accountId);
        }

        protected override int Load(List<TDadSummary> tDadItems)
        {
            Logger.Info(accountId, "Loading {0} Amazon ProductAd / Creative Daily Summaries..", tDadItems.Count);

            tdAdSummaryLoader.PrepareData(tDadItems);
            tdAdSummaryLoader.AddUpdateDependentAdSets(tDadItems);
            tdAdSummaryLoader.AddUpdateDependentTDads(tDadItems);
            tdAdSummaryLoader.AssignTDadIdToItems(tDadItems);
            var count = tdAdSummaryLoader.UpsertDailySummaries(tDadItems);
            return count;
        }
    }
}
