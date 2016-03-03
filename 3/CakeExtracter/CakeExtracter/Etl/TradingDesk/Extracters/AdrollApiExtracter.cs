﻿using AdRoll;
using AdRoll.Entities;
using CakeExtracter.Common;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public abstract class AdrollApiExtracter<T> : Extracter<T>
    {
        protected AdRollUtility _arUtility;
        protected DateRange? dateRange;
        protected string advertisableEid;

        public AdrollApiExtracter(AdRollUtility arUtility = null, DateRange? dateRange = null, string advertisableEid = null)
        {
            this._arUtility = arUtility ?? new AdRollUtility(m => Logger.Info(m), m => Logger.Warn(m));
            this.dateRange = dateRange;
            this.advertisableEid = advertisableEid;
        }
    }

    public class AdrollAdvertisablesExtracter : AdrollApiExtracter<Advertisable>
    {
        public AdrollAdvertisablesExtracter()
            : base() { }

        protected override void Extract()
        {
            Logger.Info("Extracting Advertisables");
            var advs = _arUtility.GetAdvertisables();
            Add(advs);
            End();
        }
    }

    public class AdrollDailySummariesExtracter : AdrollApiExtracter<AdrollDailySummary>
    {
        public AdrollDailySummariesExtracter(DateRange dateRange, string advertisableEid, AdRollUtility arUtility = null)
            : base(arUtility, dateRange, advertisableEid)
        { }
        //Note: this.dateRange is guaranteed to be non-null

        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from AdRoll API for ({0}) from {1:d} to {2:d}",
                        this.advertisableEid, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);

            var advSums = _arUtility.AdvertisableDailySummaries(dateRange.Value.FromDate, dateRange.Value.ToDate, advertisableEid);
            if (advSums != null)
                Add(advSums);
            End();
        }
    }
    //Note: Looks like we don't need to handle days with no stats? The AdRoll API always returns a record (possibly zero-filled)

    public class AdrollCampaignDailySummariesExtracter : AdrollApiExtracter<CampaignSummary>
    {
        public AdrollCampaignDailySummariesExtracter(DateRange dateRange, string advertisableEid, AdRollUtility arUtility = null)
            : base(arUtility, dateRange, advertisableEid)
        { }
        //Note: this.dateRange is guaranteed to be non-null

        protected override void Extract()
        {
            Logger.Info("Extracting CampaignDailySummaries from AdRoll API for {0} from {1:d} to {2:d}",
                        this.advertisableEid, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);

            foreach (var date in dateRange.Value.Dates)
            {
                var campSums = _arUtility.CampaignDailySummaries(date, this.advertisableEid);
                if (campSums != null)
                    Add(campSums);
            }
            End();
        }
    }

    public class AdrollAdDailySummariesExtracter : AdrollApiExtracter<AdSummary>
    {
        public AdrollAdDailySummariesExtracter(DateRange dateRange, string advertisableEid, AdRollUtility arUtility = null)
            : base(arUtility, dateRange, advertisableEid)
        { }
        //Note: this.dateRange is guaranteed to be non-null

        protected override void Extract()
        {
            Logger.Info("Extracting AdDailySummaries from AdRoll API for {0} from {1:d} to {2:d}",
                        this.advertisableEid, this.dateRange.Value.FromDate, this.dateRange.Value.ToDate);

            foreach (var date in dateRange.Value.Dates)
            {
                var adSums = _arUtility.AdDailySummaries(date, this.advertisableEid);
                if (adSums != null)
                    Add(adSums);
            }
            End();
        }
    }
}
