using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Commands;
using CakeExtracter.Common;
using CakeExtracter.Etl.Facebook.Exceptions;
using CakeExtracter.Etl.Facebook.Interfaces;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using DirectAgents.Domain.Entities.CPProg.Facebook;

namespace CakeExtracter.Etl.Facebook.Loaders
{
    /// <summary>
    /// Facebook loader of Reach metrics.
    /// </summary>
    public class FacebookReachMetricLoader : Loader<FbReachMetric>, IFacebookLoadingErrorHandler
    {
        private readonly IBaseRepository<FbReachMetric> metricRepository;

        /// <inheritdoc/>
        public event Action<FacebookFailedEtlException> ProcessFailedLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookReachMetricLoader"/> class.
        /// </summary>
        /// <param name="accountId">Identifier of Db account.</param>
        /// <param name="metricRepository">Database metric repository.</param>
        public FacebookReachMetricLoader(int accountId, IBaseRepository<FbReachMetric> metricRepository)
            : base(accountId)
        {
            this.metricRepository = metricRepository;
        }

        /// <inheritdoc/>
        protected override int Load(List<FbReachMetric> items)
        {
            try
            {
                Logger.Info(accountId, "Loading {0} Facebook Reach Metrics..", items.Count);
                var result = MergeMetricsWithExisted(items);
                return result ? items.Count : 0;
            }
            catch (Exception e)
            {
                OnProcessFailedLoading(e);
                return 0;
            }
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

        private void OnProcessFailedLoading(Exception e)
        {
            Logger.Error(accountId, e);
            var exception = new FacebookFailedEtlException(null, null, accountId, FbStatsTypeAgg.ReachArg, e);
            ProcessFailedLoading?.Invoke(exception);
        }
    }
}
