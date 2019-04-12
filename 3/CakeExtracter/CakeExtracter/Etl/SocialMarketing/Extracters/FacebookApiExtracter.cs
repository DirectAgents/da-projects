using System;
using System.Collections.Generic;
using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI;
using FacebookAPI.Entities;

namespace CakeExtracter.Etl.SocialMarketing.Extracters
{
    public abstract class FacebookApiExtracter<T> : Extracter<T>
    {
        protected readonly FacebookInsightsDataProvider _fbUtility;
        protected readonly DateRange? dateRange;
        protected readonly int accountId;   // in our db
        protected readonly string fbAccountId; // fb account: aka "ad account"

        public FacebookApiExtracter(FacebookInsightsDataProvider fbUtility, DateRange? dateRange, ExtAccount account, bool includeAllActions = false)
        {
            this._fbUtility = fbUtility;
            this._fbUtility.IncludeAllActions = includeAllActions;
            this.dateRange = dateRange;
            this.accountId = account.Id;
            this.fbAccountId = account.ExternalId;
        }
    }

    public class FacebookDailySummaryExtracter : FacebookApiExtracter<FBSummary>
    {
        public FacebookDailySummaryExtracter(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility, bool includeAllActions = false)
            : base(fbUtility, dateRange, account, includeAllActions)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting DailySummaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.fbAccountId, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);
            try
            {
                var fbSums = _fbUtility.GetDailyStats("act_" + fbAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
                Add(fbSums);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
            End();
        }
    }

    public class FacebookCampaignSummaryExtracter : FacebookApiExtracter<FBSummary>
    {
        public FacebookCampaignSummaryExtracter(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility, bool includeAllActions = false)
            : base(fbUtility, dateRange, account, includeAllActions)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting Campaign Summaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.fbAccountId, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);
            try
            {
                var fbSums = _fbUtility.GetDailyCampaignStats("act_" + fbAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
                Add(fbSums);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
            End();
        }
    }

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

    public class FacebookAdSummaryExtracter : FacebookApiExtracter<FBSummary>
    {
        public FacebookAdSummaryExtracter(DateRange dateRange, ExtAccount account, FacebookInsightsDataProvider fbUtility, bool includeAllActions = false)
            : base(fbUtility, dateRange, account, includeAllActions)
        { }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting Ad Summaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.fbAccountId, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);
            try
            {
                var fbSums = _fbUtility.GetDailyAdStats("act_" + fbAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
                Add(fbSums);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
            End();
        }
    }

    public class FacebookAdPreviewExtracter : Extracter<FBAdPreview>
    {
        private IEnumerable<string> fbAdIds;

        private FacebbokAdPreviewDataProvider adPreviewDataProvider;

        private readonly int accountId;

        public FacebookAdPreviewExtracter(ExtAccount account, IEnumerable<string> fbAdIds, FacebbokAdPreviewDataProvider adPreviewDataProvider)
        {
            this.fbAdIds = fbAdIds;
            accountId = account.Id;
            this.adPreviewDataProvider = adPreviewDataProvider;
        }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting Ad Previews from Facebook API for ({0})", this.accountId);
            try
            {
                var fbAds = adPreviewDataProvider.GetAdPreviews(fbAdIds);
                Add(fbAds);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
            End();
        }
    }
}
