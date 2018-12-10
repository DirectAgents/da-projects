using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class AmazonAdSetSummaryLoader : Loader<AdSetSummary>
    {
        private readonly TDAdSetSummaryLoader tdAdSetSummaryLoader;

        public AmazonAdSetSummaryLoader(int accountId)
        {
            this.tdAdSetSummaryLoader = new TDAdSetSummaryLoader(accountId);
        }

        protected override int Load(List<AdSetSummary> items)
        {
            Logger.Info(accountId, "Loading {0} Amazon AdSet Daily Summaries..", items.Count);

            tdAdSetSummaryLoader.PrepareData(items);
            tdAdSetSummaryLoader.AddUpdateDependentStrategies(items);
            tdAdSetSummaryLoader.AddUpdateDependentAdSets(items);
            tdAdSetSummaryLoader.AssignAdSetIdToItems(items);
            return tdAdSetSummaryLoader.UpsertDailySummaries(items);            
        }
    }
}
