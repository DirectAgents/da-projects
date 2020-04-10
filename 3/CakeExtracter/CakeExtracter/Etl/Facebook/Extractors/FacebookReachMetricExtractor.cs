using System;
using System.Linq;
using CakeExtracter.Commands;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook;
using FacebookAPI.Entities;
using FacebookAPI.Helpers;
using FacebookAPI.Providers;

namespace CakeExtracter.Etl.Facebook.Extractors
{
    /// <inheritdoc />
    /// <summary>
    /// Facebook extractor of Reach metrics.
    /// </summary>
    public class FacebookReachMetricExtractor : FacebookApiExtractor<FbReachMetric, FacebookInsightsReachMetricProvider>
    {
        /// <inheritdoc cref="FacebookApiExtractor{T,TProvider}"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookReachMetricExtractor" /> class.
        /// </summary>
        /// <param name="dateRange">The date range.</param>
        /// <param name="account">The account.</param>
        /// <param name="fbUtility">The fb utility.</param>
        public FacebookReachMetricExtractor(DateRange dateRange, ExtAccount account, FacebookInsightsReachMetricProvider fbUtility)
            : base(fbUtility, dateRange, account)
        {
        }

        /// <inheritdoc/>
        protected override void Extract()
        {
            Logger.Info(AccountId, $"Extracting Reach metrics from Facebook API for ({FbAccountId})");
            try
            {
                var reachMetricRows = FbUtility.GetReachMetricStats("act_" + FbAccountId);
                var reachMetricStats = reachMetricRows.Select(CreateReachMetric);
                Add(reachMetricStats);
            }
            catch (Exception ex)
            {
                OnProcessFailedExtraction(
                    this.DateRange?.FromDate,
                    this.DateRange?.ToDate,
                    this.AccountId,
                    FbStatsTypeAgg.ReachArg,
                    ex);
                Logger.Error(AccountId, ex);
            }
            End();
        }

        private static FbReachMetric CreateReachMetric(FbReachRow row)
        {
            var periodName = FacebookReachPeriodHelper.GetPeriodName(row.StartDate);
            return new FbReachMetric
            {
                Period = periodName,
                Reach = row.Reach,
            };
        }
    }
}
