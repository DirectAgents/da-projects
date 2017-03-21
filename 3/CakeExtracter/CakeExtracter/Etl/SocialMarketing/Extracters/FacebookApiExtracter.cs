using CakeExtracter.Common;
using FacebookAPI;
using FacebookAPI.Entities;
using System.Collections.Generic;
using System;

namespace CakeExtracter.Etl.SocialMarketing.Extracters
{
    public abstract class FacebookApiExtracter<T> : Extracter<T>
    {
        protected FacebookUtility _fbUtility;
        protected DateRange? dateRange;
        protected string fbAccountId; // fb account: aka "ad account"

        public FacebookApiExtracter(FacebookUtility fbUtility = null, DateRange? dateRange = null, string fbAccountId = null)
        {
            this._fbUtility = fbUtility ?? new FacebookUtility(m => Logger.Info(m), m => Logger.Warn(m));
            this.dateRange = dateRange;
            this.fbAccountId = fbAccountId;
        }
    }

    public class FacebookDailySummaryExtracter : FacebookApiExtracter<FBSummary>
    {
        public FacebookDailySummaryExtracter(DateRange dateRange, string fbAccountId, FacebookUtility fbUtility = null)
            : base(fbUtility, dateRange, fbAccountId)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.fbAccountId, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);
            try
            {
                var fbSums = _fbUtility.GetDailyStats("act_" + fbAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
                foreach (var fbSum in fbSums)
                {
                    Add(fbSum);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
    }

    public class FacebookCampaignSummaryExtracter : FacebookApiExtracter<FBSummary>
    {
        public FacebookCampaignSummaryExtracter(DateRange dateRange, string fbAccountId, FacebookUtility fbUtility = null)
            : base(fbUtility, dateRange, fbAccountId)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting Campaign Summaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.fbAccountId, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);
            try
            {
                var fbSums = _fbUtility.GetDailyCampaignStats("act_" + fbAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
                foreach (var fbSum in fbSums)
                {
                    Add(fbSum);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
    }

    public class FacebookAdSetSummaryExtracter : FacebookApiExtracter<FBSummary>
    {
        public FacebookAdSetSummaryExtracter(DateRange dateRange, string fbAccountId, FacebookUtility fbUtility = null)
            : base(fbUtility, dateRange, fbAccountId)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting AdSet Summaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.fbAccountId, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);
            try
            {
                var fbSums = _fbUtility.GetDailyAdSetStats("act_" + fbAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
                foreach (var fbSum in fbSums)
                {
                    Add(fbSum);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
    }

    public class FacebookAdSummaryExtracter : FacebookApiExtracter<FBSummary>
    {
        public FacebookAdSummaryExtracter(DateRange dateRange, string fbAccountId, FacebookUtility fbUtility = null)
            : base(fbUtility, dateRange, fbAccountId)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting Ad Summaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.fbAccountId, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);
            try
            {
                var fbSums = _fbUtility.GetDailyAdStats("act_" + fbAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
                foreach (var fbSum in fbSums)
                {
                    Add(fbSum);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
    }

    public class FacebookAdPreviewExtracter : FacebookApiExtracter<FBAdPreview>
    {
        protected IEnumerable<string> fbAdIds;

        public FacebookAdPreviewExtracter(string fbAccountId, IEnumerable<string> fbAdIds, FacebookUtility fbUtility = null)
            : base(fbUtility, null, fbAccountId)
        {
            this.fbAdIds = fbAdIds;
        }

        protected override void Extract()
        {
            Logger.Info("Extracting Ad Previews from Facebook API for ({0})", this.fbAccountId);
            //var fbAds = _fbUtility.GetAdPreviews("act_" + fbAccountId, fbAdIds);
            try
            {
                var fbAds = _fbUtility.GetAdPreviews(fbAdIds);
                foreach (var fbAd in fbAds)
                {
                    Add(fbAd);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            End();
        }
    }
}
