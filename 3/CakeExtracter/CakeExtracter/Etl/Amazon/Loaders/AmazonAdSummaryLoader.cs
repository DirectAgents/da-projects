using System;
using System.Collections.Generic;
using CakeExtracter.Etl.Amazon.Exceptions;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    /// <summary>
    /// Summary loader for amazon AD level.
    /// </summary>
    /// <seealso cref="BaseAmazonLevelLoader{TDadSummary, TDadSummaryMetric}" />
    public class AmazonAdSummaryLoader : BaseAmazonLevelLoader<TDadSummary, TDadSummaryMetric>
    {
        private readonly TDadSummaryLoader summaryItemsLoader;

        public AmazonAdSummaryLoader(int accountId)
            : base(accountId)
        {
            summaryItemsLoader = new TDadSummaryLoader(accountId);
        }

        protected override string LevelName => AmazonJobLevels.Creative;

        protected override object LockerObject => SafeContextWrapper.AdLocker;

        protected override void EnsureRelatedItems(List<TDadSummary> tDadSummaryItems)
        {
            summaryItemsLoader.PrepareData(tDadSummaryItems);
            summaryItemsLoader.AddUpdateDependentStratagies(tDadSummaryItems);
            summaryItemsLoader.AddUpdateDependentAdSets(tDadSummaryItems);
            summaryItemsLoader.AddUpdateDependentTDads(tDadSummaryItems);
            summaryItemsLoader.AssignTDadIdToItems(tDadSummaryItems);
        }

        protected override void SetSummaryMetricEntityId(TDadSummary summary, SummaryMetric summaryMetric)
        {
            summaryMetric.EntityId = summary.TDadId;
        }

        protected override FailedStatsLoadingException GetFailedStatsLoadingException(Exception e, List<TDadSummary> items)
        {
            var exception = base.GetFailedStatsLoadingException(e, items);
            exception.ByAd = true;
            return exception;
        }
    }
}
