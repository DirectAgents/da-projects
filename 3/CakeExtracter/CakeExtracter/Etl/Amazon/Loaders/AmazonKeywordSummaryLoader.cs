using System;
using System.Collections.Generic;
using CakeExtracter.Etl.Amazon.Exceptions;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.Amazon.Loaders
{
    /// <inheritdoc />
    /// <summary>
    /// Summary loader for amazon Keyword level.
    /// </summary>
    public class AmazonKeywordSummaryLoader : BaseAmazonLevelLoader<KeywordSummary, KeywordSummaryMetric>
    {
        private readonly KeywordSummaryLoader summaryLoader;

        /// <inheritdoc cref="BaseAmazonLevelLoader{KeywordSummary, KeywordSummaryMetric}" />
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonKeywordSummaryLoader" /> class.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        public AmazonKeywordSummaryLoader(int accountId)
            : base(accountId)
        {
            summaryLoader = new KeywordSummaryLoader(accountId);
        }

        /// <summary>
        /// Gets the name of amazon job information level.
        /// </summary>
        /// <value>
        /// The name of the level.
        /// </value>
        protected override string LevelName => AmazonJobLevels.Keyword;

        /// <summary>
        /// Gets the locker object for multithreading operations.
        /// </summary>
        /// <value>
        /// The locker object.
        /// </value>
        protected override object LockerObject => SafeContextWrapper.KeywordLocker;

        /// <summary>
        /// Ensures the related items.
        /// </summary>
        /// <param name="keywordItems">The keyword items.</param>
        protected override void EnsureRelatedItems(List<KeywordSummary> keywordItems)
        {
            summaryLoader.PrepareData(keywordItems);
            summaryLoader.AddUpdateDependentStrategies(keywordItems);
            summaryLoader.AddUpdateDependentAdSets(keywordItems);
            summaryLoader.AddUpdateDependentKeywords(keywordItems);
            summaryLoader.AssignKeywordIdToItems(keywordItems);
        }

        /// <summary>
        /// Sets the summary metric entity identifier.
        /// </summary>
        /// <param name="summary">The summary.</param>
        /// <param name="summaryMetric">The summary metric.</param>
        protected override void SetSummaryMetricEntityId(KeywordSummary summary, SummaryMetric summaryMetric)
        {
            summaryMetric.EntityId = summary.KeywordId;
        }

        protected override FailedStatsLoadingException GetFailedStatsLoadingException(Exception e, List<KeywordSummary> items)
        {
            var exception = base.GetFailedStatsLoadingException(e, items);
            exception.ByKeyword = true;
            return exception;
        }
    }
}
