using System;
using System.Collections.Generic;
using CakeExtracter.Etl.Amazon.Exceptions;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    public class AmazonSearchTermSummaryLoader : BaseAmazonLevelLoader<SearchTermSummary, SearchTermSummaryMetric>
    {
        private readonly SearchTermSummaryLoader summaryLoader;

        public AmazonSearchTermSummaryLoader(int accountId)
            : base(accountId)
        {
            summaryLoader = new SearchTermSummaryLoader(accountId);
        }

        protected override string LevelName => AmazonJobLevels.SearchTerm;

        protected override object LockerObject => SafeContextWrapper.SearchTermLocker;

        protected override void EnsureRelatedItems(List<SearchTermSummary> searchTermItems)
        {
            summaryLoader.PrepareData(searchTermItems);
            summaryLoader.AddUpdateDependentStrategies(searchTermItems);
            summaryLoader.AddUpdateDependentAdSets(searchTermItems);
            summaryLoader.AddUpdateDependentKeywords(searchTermItems);
            summaryLoader.AddUpdateDependentSearchTerms(searchTermItems);
            summaryLoader.AssignSearchTermIdToItems(searchTermItems);
        }

        protected override void SetSummaryMetricEntityId(SearchTermSummary summary, SummaryMetric summaryMetric)
        {
            summaryMetric.EntityId = summary.SearchTermId;
        }

        protected override FailedStatsLoadingException GetFailedStatsLoadingException(Exception e, List<SearchTermSummary> items)
        {
            var exception = base.GetFailedStatsLoadingException(e, items);
            exception.BySearchTerm = true;
            return exception;
        }
    }
}
