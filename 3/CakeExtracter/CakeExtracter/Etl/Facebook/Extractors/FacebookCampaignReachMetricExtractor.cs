using System;
using System.Linq;
using CakeExtracter.Commands;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook;
using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;

using FacebookAPI.Entities;
using FacebookAPI.Helpers;
using FacebookAPI.Providers;

namespace CakeExtracter.Etl.Facebook.Extractors
{
    /// <inheritdoc />
    /// <summary>
    /// Facebook extractor of Reach metrics for Campaign and Monthly level.
    /// </summary>
    public class FacebookCampaignReachMetricExtractor : FacebookApiExtractor<FbCampaignReachMetric, FacebookInsightsReachMetricProvider>
    {
        /// <inheritdoc cref="FacebookApiExtractor{T,TProvider}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookCampaignReachMetricExtractor" /> class.
        /// </summary>
        /// <param name="dateRange">The date range.</param>
        /// <param name="account">The account.</param>
        /// <param name="fbUtility">The fb utility.</param>
        public FacebookCampaignReachMetricExtractor(DateRange dateRange, ExtAccount account, FacebookInsightsReachMetricProvider fbUtility)
            : base(fbUtility, dateRange, account)
        {
        }

        /// <inheritdoc/>
        protected override void Extract()
        {
            Logger.Info(AccountId, $"Extracting Reach metrics from Facebook API for ({FbAccountId})");
            try
            {
                var reachMetricRows = FbUtility.GetCampaignReachMetricStats("act_" + FbAccountId);
                var reachMetricStats = reachMetricRows.Select(CreateReachMetric);
                Add(reachMetricStats);
            }
            catch (Exception ex)
            {
                OnProcessFailedExtraction(
                    this.DateRange?.FromDate,
                    this.DateRange?.ToDate,
                    this.AccountId,
                    FbStatsTypeAgg.CampaignReachArg,
                    ex);
                Logger.Error(AccountId, ex);
            }
            End();
        }

        private FbCampaignReachMetric CreateReachMetric(FbCampaignReachRow row)
        {
            var periodName = FacebookReachPeriodHelper.GetMonthPeriodName(row.StartDate);
            return new FbCampaignReachMetric
                   {
                       Campaign = new FbCampaign
                                  {
                                      AccountId = this.AccountId,
                                      Name = row.CampaignName,
                                      ExternalId = row.CampaignId,
                                  },
                       Period = periodName,
                       Reach = row.Reach,
                       Frequency = row.Frequency,
                   };
        }
    }
}