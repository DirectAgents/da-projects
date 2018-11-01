using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    class AmazonSearchTermSummaryLoader : Loader<SearchTermSummary>
    {
        private readonly SearchTermSummaryLoader searchTermSummaryLoader;

        public AmazonSearchTermSummaryLoader(int accountId)
        {
            searchTermSummaryLoader = new SearchTermSummaryLoader(accountId);
        }

        protected override int Load(List<SearchTermSummary> items)
        {
            Logger.Info(accountId, "Loading {0} Amazon Search term data: ", items.Count);

            searchTermSummaryLoader.PrepareData(items);
            searchTermSummaryLoader.AddUpdateDependentStrategies(items);
            searchTermSummaryLoader.AddUpdateDependentAdSets(items);
            searchTermSummaryLoader.AddUpdateDependentKeywords(items);
            searchTermSummaryLoader.AddUpdateDependentSearchTerms(items);
            searchTermSummaryLoader.AssignSearchTermIdToItems(items);
            var count = searchTermSummaryLoader.UpsertDailySummaries(items);
            return count;
        }
    }
}
