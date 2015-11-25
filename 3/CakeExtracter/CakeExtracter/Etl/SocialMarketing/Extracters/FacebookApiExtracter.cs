using CakeExtracter.Common;
using FacebookAPI;
using FacebookAPI.Entities;

namespace CakeExtracter.Etl.SocialMarketing.Extracters
{
    public abstract class FacebookApiExtracter<T> : Extracter<T>
    {
        protected FacebookUtility _fbUtility;
        protected DateRange? dateRange;
        protected string adAccountId;

        public FacebookApiExtracter(FacebookUtility fbUtility = null, DateRange? dateRange = null, string adAccountId = null)
        {
            this._fbUtility = fbUtility ?? new FacebookUtility(m => Logger.Info(m), m => Logger.Warn(m));
            this.dateRange = dateRange;
            this.adAccountId = adAccountId;
        }
    }

    public class FacebookDailySummariesExtracter : FacebookApiExtracter<FBSummary>
    {
        public FacebookDailySummariesExtracter(DateRange dateRange, string adAccountId, FacebookUtility fbUtility = null)
            : base(fbUtility, dateRange, adAccountId)
        { }

        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from Facebook API for ({0}) from {1:d} to {2:d}",
                        this.adAccountId, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);
            var fbSums = _fbUtility.GetDailyStats("act_" + adAccountId, dateRange.Value.FromDate, dateRange.Value.ToDate);
            foreach (var fbSum in fbSums)
            {
                Add(fbSum);
            }
            End();
        }
    }
}
