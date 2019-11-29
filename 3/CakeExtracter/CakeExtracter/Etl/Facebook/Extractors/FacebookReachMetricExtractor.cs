using System;
using System.Linq;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Facebook;
using FacebookAPI.Entities;
using FacebookAPI.Helpers;
using FacebookAPI.Providers;

namespace CakeExtracter.Etl.Facebook.Extractors
{
    public class FacebookReachMetricExtractor : FacebookApiExtractor<FbReachMetric, FacebookInsightsReachMetricProvider>
    {
        public FacebookReachMetricExtractor(
            DateRange dateRange, ExtAccount account, FacebookInsightsReachMetricProvider fbUtility)
            : base(fbUtility, dateRange, account)
        {
        }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting Reach metrics from Facebook API for ({0}) from {1:d} to {2:d}",
                fbAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
            try
            {
                var reachMetricRows = _fbUtility.GetReachMetricStats("act_" + fbAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
                var reachMetricStats = reachMetricRows.Select(CreateReachMetric);
                Add(reachMetricStats);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
            End();
        }

        private static FbReachMetric CreateReachMetric(FbReachRow row)
        {
            //var period = new DateRange(row.StartDate, row.EndDate);
            var periodName = FacebookReachPeriodHelper.GetPeriodName(row.StartDate);
            return new FbReachMetric
            {
                Period = periodName,
                Reach = row.Reach,
            };
        }
    }
}
