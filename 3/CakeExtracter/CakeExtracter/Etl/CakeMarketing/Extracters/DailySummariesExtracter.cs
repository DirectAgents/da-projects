using System.Linq;
using System.Threading.Tasks;
using CakeExtracter.CakeMarketingApi;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;

namespace CakeExtracter.Etl.CakeMarketing.Extracters
{
    public class DailySummariesExtracter : Extracter<OfferDailySummary>
    {
        private readonly DateRange dateRange;
        private readonly int advertiserId;

        public DailySummariesExtracter(DateRange dateRange, int advertiserId)
        {
            this.dateRange = dateRange;
            this.advertiserId = advertiserId;
        }

        protected override void Extract()
        {
            Logger.Info("Getting offerIds for advertiserId={0}", advertiserId);
            
            var offerIds = CakeMarketingUtility.OfferIds(advertiserId);
            
            Logger.Info("Extracting DailySummaries for {0} offerIds between {2} and {3}: {1}", offerIds.Count, string.Join(", ", offerIds), dateRange.FromDate.ToShortDateString(), dateRange.ToDate.ToShortDateString());
            
            Parallel.ForEach(offerIds, offerId =>
                {
                    var dailySummaries = CakeMarketingUtility.DailySummaries(dateRange, advertiserId, offerId);

                    Logger.Info("Extracted {0} DailySummaries for offerId={1}", dailySummaries.Count, offerId);

                    Add(dailySummaries.Select(c => new OfferDailySummary(offerId, c)));
                });
            End();
        }
    }
}
