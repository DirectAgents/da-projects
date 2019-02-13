using System.Collections.Generic;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders
{
    /// <summary>
    /// Summary loader for amazon ADSET level.
    /// </summary>
    /// <seealso cref="CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders.BaseAmazonLevelLoader{DirectAgents.Domain.Entities.CPProg.AdSetSummary, DirectAgents.Domain.Entities.CPProg.AdSetSummaryMetric}" />
    public class AmazonAdSetSummaryLoader : BaseAmazonLevelLoader<AdSetSummary, AdSetSummaryMetric>
    {
        private readonly TDAdSetSummaryLoader summaryItemsLoader;

        /// <summary>
        /// Gets the name of amazon job information level.
        /// </summary>
        /// <value>
        /// The name of the level.
        /// </value>
        protected override string LevelName => AmazonJobLevels.adSet;

        /// <summary>
        /// Gets the locker object for multithreading operations.
        /// </summary>
        /// <value>
        /// The locker object.
        /// </value>
        protected override object LockerObject => SafeContextWrapper.AdSetLocker;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonAdSetSummaryLoader"/> class.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        public AmazonAdSetSummaryLoader(int accountId)
            : base(accountId)
        {
            summaryItemsLoader = new TDAdSetSummaryLoader(accountId);
        }

        /// <summary>
        /// Ensures the related entities in db by it's summary items.
        /// </summary>
        /// <param name="items">The items.</param>
        protected override void EnsureRelatedItems(List<AdSetSummary> items)
        {
            summaryItemsLoader.PrepareData(items);
            summaryItemsLoader.AddUpdateDependentStrategies(items);
            summaryItemsLoader.AddUpdateDependentAdSets(items);
            summaryItemsLoader.AssignAdSetIdToItems(items);
        }

        /// <summary>
        /// Sets the summary metric entity identifier.
        /// </summary>
        /// <param name="summary">The summary.</param>
        /// <param name="summaryMetric">The summary metric.</param>
        protected override void SetSummaryMetricEntityId(AdSetSummary summary, SummaryMetric summaryMetric)
        {
            summaryMetric.EntityId = summary.AdSetId;
        }
    }
}
