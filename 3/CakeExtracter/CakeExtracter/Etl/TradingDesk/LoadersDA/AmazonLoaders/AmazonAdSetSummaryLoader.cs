using System.Collections.Generic;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    public class AmazonAdSetSummaryLoader : BaseAmazonLevelLoader<AdSetSummary, AdSetSummaryMetric>
    {
        private readonly TDAdSetSummaryLoader summaryItemsLoader;

        protected override string LevelName => AmazonJobLevels.adSet;

        protected override object LockerObject => SafeContextWrapper.AdSetLocker;

        public AmazonAdSetSummaryLoader(int accountId)
            : base(accountId)
        {
            summaryItemsLoader = new TDAdSetSummaryLoader(accountId);
        }

        protected override void EnsureRelatedItems(List<AdSetSummary> items)
        {
            summaryItemsLoader.PrepareData(items);
            summaryItemsLoader.AddUpdateDependentStrategies(items);
            summaryItemsLoader.AddUpdateDependentAdSets(items);
            summaryItemsLoader.AssignAdSetIdToItems(items);
        }

        protected override void SetSummaryMetricEntityId(AdSetSummary summary, SummaryMetric summaryMetric)
        {
            summaryMetric.EntityId = summary.AdSetId;
        }
    }
}
