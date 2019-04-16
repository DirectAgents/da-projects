using System;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI;
using FacebookAPI.Entities;

namespace CakeExtracter.Etl.SocialMarketing.Extracters
{
    public class FacebookAdSetSummaryExtracter : FacebookApiExtracter<FBSummary>
    {
        public FacebookAdSetSummaryExtracter(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility, bool includeAllActions = false)
            : base(fbUtility, dateRange, account, includeAllActions)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting AdSet Summaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.fbAccountId, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);
            try
            {
                var fbSums = _fbUtility.GetDailyAdSetStats("act_" + fbAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
                Add(fbSums);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
            End();
        }
    }
}
