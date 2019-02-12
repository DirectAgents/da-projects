using System.Collections.Generic;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    public class AmazonCampaignSummaryLoader : BaseAmazonLevelLoader<StrategySummary, StrategySummaryMetric>
    {
        private readonly TDStrategySummaryLoader summaryItemsLoader;

        public AmazonCampaignSummaryLoader(int accountId)
            : base(accountId)
        {
            summaryItemsLoader = new TDStrategySummaryLoader(accountId);
        }

        protected override string LevelName => AmazonJobLevels.strategy;

        protected override object LockerObject => SafeContextWrapper.StrategyLocker;

        protected override void EnsureRelatedItems(List<StrategySummary> summaryItems)
        {
            summaryItemsLoader.PrepareData(summaryItems);
            summaryItemsLoader.AddUpdateDependentStrategies(summaryItems);
            summaryItemsLoader.AssignStrategyIdToItems(summaryItems);
        }

        protected override void SetSummaryMetricEntityId(StrategySummary summary, SummaryMetric summaryMetric)
        {
            summaryMetric.EntityId = summary.StrategyId;
        }
    }
}
