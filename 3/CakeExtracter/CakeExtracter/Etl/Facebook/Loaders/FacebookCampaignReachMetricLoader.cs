using System;
using System.Collections.Generic;
using System.Linq;

using CakeExtracter.Commands;
using CakeExtracter.Etl.Facebook.Exceptions;
using CakeExtracter.Etl.Facebook.Interfaces;
using CakeExtracter.Etl.Facebook.Loaders.EntitiesLoaders;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;

using DirectAgents.Domain.Entities.CPProg.Facebook;

namespace CakeExtracter.Etl.Facebook.Loaders
{
    public class FacebookCampaignReachMetricLoader : Loader<FbCampaignReachMetric>, IFacebookLoadingErrorHandler
    {
        private readonly IBaseRepository<FbCampaignReachMetric> metricRepository;

        private readonly FacebookCampaignsLoader fbCampaignsLoader;

        /// <inheritdoc/>
        public event Action<FacebookFailedEtlException> ProcessFailedLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookCampaignReachMetricLoader"/> class.
        /// </summary>
        /// <param name="accountId">Identifier of Db account.</param>
        /// <param name="metricRepository">Database metric repository.</param>
        public FacebookCampaignReachMetricLoader(int accountId, IBaseRepository<FbCampaignReachMetric> metricRepository)
            : base(accountId)
        {
            this.metricRepository = metricRepository;
            fbCampaignsLoader = new FacebookCampaignsLoader();
        }

        /// <inheritdoc/>
        protected override int Load(List<FbCampaignReachMetric> items)
        {
            try
            {
                EnsureCampaignsEntitiesData(items);
                Logger.Info(accountId, "Loading {0} Facebook Campaign Reach Metrics..", items.Count);
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
        private bool MergeMetricsWithExisted(List<FbCampaignReachMetric> items)
        {
            var result = metricRepository.MergeItems(items);
            LogMergedEntities(items, metricRepository.EntityName);
            return result;
        }

        private void EnsureCampaignsEntitiesData(List<FbCampaignReachMetric> items)
        {
            var fbCampaigns = items.Select(item => item.Campaign).Where(item => item != null).ToList();
            fbCampaignsLoader.AddUpdateDependentEntities(fbCampaigns);
            items.ForEach(item =>
            {
                item.CampaignId = item.Campaign.Id;
            });
        }

        private void LogMergedEntities(IEnumerable<FbCampaignReachMetric> items, string entitiesName)
        {
            Logger.Info(accountId, $"{entitiesName} were merged: {items.Count()}.");
        }

        private void OnProcessFailedLoading(Exception e)
        {
            Logger.Error(accountId, e);
            var exception = new FacebookFailedEtlException(null, null, accountId, FbStatsTypeAgg.CampaignReachArg, e);
            ProcessFailedLoading?.Invoke(exception);
        }
    }
}