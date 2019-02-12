using System.Collections.Generic;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    //Assumed that clean before update done!!!!
    internal class AmazonAdSummaryLoader : BaseAmazonLevelLoader<TDadSummary, TDadSummaryMetric>
    {
        private readonly TDadSummaryLoader summaryItemsLoader;

        public AmazonAdSummaryLoader(int accountId)
            : base(accountId)
        {
            summaryItemsLoader = new TDadSummaryLoader(accountId);
        }

        protected override string LevelName => AmazonJobLevels.creative;

        protected override object LockerObject => SafeContextWrapper.AdLocker;

        protected override void EnsureRelatedItems(List<TDadSummary> tDadSummaryItems)
        {
            summaryItemsLoader.PrepareData(tDadSummaryItems);
            summaryItemsLoader.AddUpdateDependentAdSets(tDadSummaryItems);
            summaryItemsLoader.AddUpdateDependentTDads(tDadSummaryItems);
            summaryItemsLoader.AssignTDadIdToItems(tDadSummaryItems);
        }

        protected override void SetSummaryMetricEntityId(TDadSummary summary, SummaryMetric summaryMetric)
        {
            summaryMetric.EntityId = summary.TDadId;
        }
    }
}
