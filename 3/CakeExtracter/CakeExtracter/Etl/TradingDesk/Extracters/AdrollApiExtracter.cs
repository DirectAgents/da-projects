using AdRoll;
using AdRoll.Entities;
using CakeExtracter.Common;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public class AdrollAdvertisablesExtracter : Extracter<Advertisable>
    {
        private AdRollUtility _arUtility;

        public AdrollAdvertisablesExtracter()
        {
            _arUtility = new AdRollUtility(m => Logger.Info(m), m => Logger.Warn(m));
        }

        protected override void Extract()
        {
            Logger.Info("Extracting Advertisables");
            var advs = _arUtility.GetAdvertisables();
            Add(advs);
            End();
        }
    }

    public class AdrollAdDailySummariesExtracter : Extracter<AdSummary>
    {
        private readonly DateRange dateRange;
        private readonly string advertisableEid;

        private AdRollUtility _arUtility;

        public AdrollAdDailySummariesExtracter(DateRange dateRange, string advertiseableEid)
        {
            this.dateRange = dateRange;
            this.advertisableEid = advertiseableEid;
            _arUtility = new AdRollUtility(m => Logger.Info(m), m => Logger.Warn(m));
        }

        protected override void Extract()
        {
            Logger.Info("Extracting AdDailySummaries from AdRoll API for {0} from {1:d} to {2:d}",
                        this.advertisableEid, this.dateRange.FromDate, this.dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                var adSums = _arUtility.AdSummaries(date, this.advertisableEid);
                Add(adSums);
            }
            End();
        }
    }

    public class AdrollDailySummariesExtracter : Extracter<AdrollDailySummary>
    {
        private readonly DateRange dateRange;
        private readonly string advertisableEid;

        private AdRollUtility _arUtility;

        public AdrollDailySummariesExtracter(DateRange dateRange, string advertiseableEid)
        {
            this.dateRange = dateRange;
            this.advertisableEid = advertiseableEid;
            _arUtility = new AdRollUtility(m => Logger.Info(m), m => Logger.Warn(m));
        }

        protected override void Extract()
        {
            Logger.Info("Extracting DailySummaries from AdRoll API for {0} from {1:d} to {2:d}",
                        this.advertisableEid, this.dateRange.FromDate, this.dateRange.ToDate);

            var advSums = _arUtility.AdvertisableSummaries(dateRange.FromDate, dateRange.ToDate, advertisableEid);
            Add(advSums);
            End();
        }
    }
    //Note: Looks like we don't need to handle days with no stats? The AdRoll API always returns a record (possibly zero-filled)
}
