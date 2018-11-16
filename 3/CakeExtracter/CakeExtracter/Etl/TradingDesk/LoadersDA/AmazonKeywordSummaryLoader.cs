using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class AmazonKeywordSummaryLoader : Loader<KeywordSummary>
    {
        private readonly KeywordSummaryLoader keywordSummaryLoader;

        public AmazonKeywordSummaryLoader(int accountId)
        {
            keywordSummaryLoader = new KeywordSummaryLoader(accountId);
        }

        protected override int Load(List<KeywordSummary> keywordItems)
        {
            Logger.Info(accountId, "Loading {0} Amazon Keyword Daily Summaries..", keywordItems.Count);

            keywordSummaryLoader.PrepareData(keywordItems);
            keywordSummaryLoader.AddUpdateDependentStrategies(keywordItems);
            keywordSummaryLoader.AddUpdateDependentAdSets(keywordItems);
            keywordSummaryLoader.AddUpdateDependentKeywords(keywordItems);
            keywordSummaryLoader.AssignKeywordIdToItems(keywordItems);
            var count = keywordSummaryLoader.UpsertDailySummaries(keywordItems);
            return count;
        }
    }
}
