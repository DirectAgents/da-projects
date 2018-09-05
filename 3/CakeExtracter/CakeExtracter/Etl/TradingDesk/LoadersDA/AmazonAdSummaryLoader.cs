using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class AmazonAdSummaryLoader : Loader<TDadSummary>
    {
        private TDadSummaryLoader tdAdSummaryLoader;

        public AmazonAdSummaryLoader(int accountId)
        {
            this.tdAdSummaryLoader = new TDadSummaryLoader(accountId);
        }

        protected override int Load(List<TDadSummary> tDadItems)
        {
            Logger.Info(accountId, "Loading {0} Amazon ProductAd / Creative data: ", tDadItems.Count);

            tdAdSummaryLoader.AddUpdateDependentTDads(tDadItems);
            tdAdSummaryLoader.AssignTDadIdToItems(tDadItems);
            var count = tdAdSummaryLoader.UpsertDailySummaries(tDadItems);
            return count;
        }
    }
}
