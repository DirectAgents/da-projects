using System.Collections.Generic;
using System.Linq;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.Facebook;

namespace CakeExtracter.Etl.Facebook.Loaders
{
    public class FacebookReachMetricLoader : Loader<FbReachMetric>
    {
        private readonly IBaseRepository<FbReachMetric> metricRepository;

        public FacebookReachMetricLoader(int accountId, IBaseRepository<FbReachMetric> metricRepository)
            : base(accountId)
        {
            this.metricRepository = metricRepository;
        }

        /// <inheritdoc/>
        protected override int Load(List<FbReachMetric> items)
        {
            Logger.Info(accountId, "Loading {0} Facebook Reach Metrics..", items.Count);
            var result = MergeMetricsWithExisted(items);
            return result ? items.Count : 0;
        }

        /// <summary>
        /// Merges (inserts or updates) Reach metrics with existed in DB.
        /// </summary>
        /// <param name="items">Reach metric items for merge.</param>
        /// <returns>True if successfully merged.</returns>
        private bool MergeMetricsWithExisted(List<FbReachMetric> items)
        {
            items.ForEach(SetMetricParents);
            var result = metricRepository.MergeItems(items);
            LogMergedEntities(items, metricRepository.EntityName);
            return result;
        }

        /// <summary>
        /// Sets DB identifiers of parent account for Reach metric.
        /// </summary>
        /// <param name="metric">Reach metric.</param>
        private void SetMetricParents(FbReachMetric metric)
        {
            metric.AccountId = accountId;
        }

        private void LogMergedEntities(IEnumerable<FbReachMetric> items, string entitiesName)
        {
            Logger.Info(accountId, $"{entitiesName} were merged: {items.Count()}.");
        }
    }
}
