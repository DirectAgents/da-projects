using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class AmazonAdSetSummaryLoader : Loader<AdSetSummary>
    {
        private TDAdSetSummaryLoader tdAdSetSummaryLoader;

        public int AccountId { get { return tdAdSetSummaryLoader.AccountId; } }

        public AmazonAdSetSummaryLoader(int accountId = -1)
        {
            this.tdAdSetSummaryLoader = new TDAdSetSummaryLoader(accountId);
        }

        protected override int Load(List<AdSetSummary> items)
        {
            Logger.Info(AccountId, "Loading {0} Amazon AdSet and Summary data:", items.Count);

            tdAdSetSummaryLoader.AddUpdateDependentStrategies(items);
            tdAdSetSummaryLoader.AddUpdateDependentAdSets(items);
            tdAdSetSummaryLoader.AssignAdSetIdToItems(items);
            return tdAdSetSummaryLoader.UpsertDailySummaries(items);            
        }
    }
}
