using System;
using Apple;
using CakeExtracter.Common;

namespace CakeExtracter.Etl.SearchMarketing.Extracters
{
    public class AppleApiExtracter : Extracter<AppleStatGroup>
    {
        protected readonly AppleAdsUtility appleAdsUtility;
        protected readonly DateRange dateRange;
        protected readonly string orgId;

        public AppleApiExtracter(AppleAdsUtility appleAdsUtility, DateRange dateRange, string orgId)
        {
            this.appleAdsUtility = appleAdsUtility;
            this.dateRange = dateRange;
            this.orgId = orgId;
        }

        //TODO: Handle gaps in the stats. Somehow pass deletion items... how to do this for all campaigns?

        protected override void Extract()
        {
            Logger.Info("Extracting daily stats from AppleAds API for ({0}) from {1:d} to {2:d}",
                orgId, dateRange.FromDate, dateRange.ToDate);
            try
            {
                var appleStatGroups = appleAdsUtility.GetCampaignDailyStats(orgId, dateRange.FromDate, dateRange.ToDate);
                Add(appleStatGroups);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
    }
}
