using System.Collections.Generic;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    public class AmazonKeywordSummaryLoader : BaseAmazonLevelLoader<KeywordSummary, KeywordSummaryMetric>
    {
        private readonly KeywordSummaryLoader summaryLoader;

        public AmazonKeywordSummaryLoader(int accountId)
            : base(accountId)
        {
            summaryLoader = new KeywordSummaryLoader(accountId);
        }

        protected override string LevelName => AmazonJobLevels.keyword;

        protected override object LockerObject => SafeContextWrapper.KeywordLocker;

        protected override void EnsureRelatedItems(List<KeywordSummary> keywordItems)
        {
            summaryLoader.PrepareData(keywordItems);
            summaryLoader.AddUpdateDependentStrategies(keywordItems);
            summaryLoader.AddUpdateDependentAdSets(keywordItems);
            summaryLoader.AddUpdateDependentKeywords(keywordItems);
            summaryLoader.AssignKeywordIdToItems(keywordItems);
        }

        protected override void SetSummaryMetricEntityId(KeywordSummary summary, SummaryMetric summaryMetric)
        {
            summaryMetric.EntityId = summary.KeywordId;
        }
    }
}
