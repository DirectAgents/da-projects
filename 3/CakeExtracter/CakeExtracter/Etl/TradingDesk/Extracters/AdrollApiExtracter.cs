using AdRoll;
using AdRoll.Entities;
using CakeExtracter.Common;

namespace CakeExtracter.Etl.TradingDesk.Extracters
{
    public class AdrollApiExtracter : Extracter<AdSummary>
    {
        private readonly DateRange dateRange;
        private readonly string advertiseableEid;

        private AdRollUtility _arUtility;

        public AdrollApiExtracter(DateRange dateRange, string advertiseableEid)
        {
            this.dateRange = dateRange;
            this.advertiseableEid = advertiseableEid;
            _arUtility = new AdRollUtility(m => Logger.Info(m), m => Logger.Warn(m));
        }

        protected override void Extract()
        {
            Logger.Info("Extracting AdDailySummaries from AdRoll API for {0} from {1:d} to {2:d}",
                        this.advertiseableEid, this.dateRange.FromDate, this.dateRange.ToDate);

            foreach (var date in dateRange.Dates)
            {
                var adSums = _arUtility.AdSummaries(date, this.advertiseableEid);
                Add(adSums);
            }
            End();
        }
    }
}
