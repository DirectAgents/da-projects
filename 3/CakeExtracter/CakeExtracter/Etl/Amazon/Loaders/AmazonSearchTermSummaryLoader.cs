﻿using System;
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
    /// Summary loader for amazon Search term level.
    /// </summary>
    public class AmazonSearchTermSummaryLoader : BaseAmazonLevelLoader<SearchTermSummary, SearchTermSummaryMetric>
    {
        private readonly SearchTermSummaryLoader summaryLoader;

        /// <inheritdoc cref="BaseAmazonLevelLoader{SearchTermSummary, SearchTermSummaryMetric}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonSearchTermSummaryLoader" /> class.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        public AmazonSearchTermSummaryLoader(int accountId)
            : base(accountId)
        {
            summaryLoader = new SearchTermSummaryLoader(accountId);
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the name of amazon job information level.
        /// </summary>
        /// <value>
        /// The name of the level.
        /// </value>
        protected override string LevelName => AmazonJobLevels.SearchTerm;

        /// <inheritdoc />
        /// <summary>
        /// Gets the locker object for multithreading operations.
        /// </summary>
        /// <value>
        /// The locker object.
        /// </value>
        protected override object LockerObject => SafeContextWrapper.SearchTermLocker;

        /// <inheritdoc />
        /// <summary>
        /// Ensures the related items.
        /// </summary>
        /// <param name="searchTermItems">The search term items.</param>
        protected override void EnsureRelatedItems(List<SearchTermSummary> searchTermItems)
        {
            summaryLoader.PrepareData(searchTermItems);
            summaryLoader.AddUpdateDependentStrategies(searchTermItems);
            summaryLoader.AddUpdateDependentAdSets(searchTermItems);
            summaryLoader.AddUpdateDependentKeywords(searchTermItems);
            summaryLoader.AddUpdateDependentSearchTerms(searchTermItems);
            summaryLoader.AssignSearchTermIdToItems(searchTermItems);
        }

        /// <inheritdoc />
        /// <summary>
        /// Sets the summary metric entity identifier.
        /// </summary>
        /// <param name="summary">The summary.</param>
        /// <param name="summaryMetric">The summary metric.</param>
        protected override void SetSummaryMetricEntityId(SearchTermSummary summary, SummaryMetric summaryMetric)
        {
            summaryMetric.EntityId = summary.SearchTermId;
        }

        /// <summary>
        /// Gets exception with additional info about process for relaunch mechanism.
        /// </summary>
        /// <param name="e">Base exception.</param>
        /// <param name="items">List of stats items.</param>
        /// <returns>Exception with additional info about process for relaunch mechanism.</returns>
        protected override FailedStatsLoadingException GetFailedStatsLoadingException(Exception e, List<SearchTermSummary> items)
        {
            var exception = base.GetFailedStatsLoadingException(e, items);
            exception.BySearchTerm = true;
            return exception;
        }
    }
}
