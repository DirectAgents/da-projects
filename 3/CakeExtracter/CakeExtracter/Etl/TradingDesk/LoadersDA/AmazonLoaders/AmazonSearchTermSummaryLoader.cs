using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    class AmazonSearchTermSummaryLoader : BaseAmazonLevelLoader<SearchTermSummary>
    {
        private readonly SearchTermSummaryLoader searchTermSummaryLoader;

        public AmazonSearchTermSummaryLoader(int accountId)
            : base(accountId)
        {
            searchTermSummaryLoader = new SearchTermSummaryLoader(accountId);
        }

        protected override int Load(List<SearchTermSummary> items)
        {
            Logger.Info(accountId, "Loading {0} Amazon Search Term Daily Summaries..", items.Count);

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
