using System.Threading.Tasks;
using CakeExtracter.CakeMarketingApi;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;

namespace CakeExtracter.Etl.CakeMarketing.Extracters
{
    public class ClicksExtracter : Extracter<Click>
    {
        private readonly DateRange dateRange;
        private readonly int advertiserId;

        public ClicksExtracter(DateRange dateRange, int advertiserId)
        {
            this.dateRange = dateRange;
            this.advertiserId = advertiserId;
        }

        protected override void Extract()
        {
            Logger.Info("Getting offerIds for advertiserId={0}", advertiserId);
            var offerIds = CakeMarketingUtility.OfferIds(advertiserId);
            Logger.Info("Extracting Clicks for {0} offerIds between {2} and {3}: {1}",
                            offerIds.Count,
                            string.Join(", ", offerIds), dateRange.FromDate.ToShortDateString(),
                            dateRange.ToDate.ToShortDateString());
            Parallel.ForEach(offerIds, offerId =>
            {
                int rowCount;
                var clicks = CakeMarketingUtility.Clicks(dateRange, advertiserId, offerId, out rowCount);
                Logger.Info("Extracted {0} Clicks for offerId={1}", clicks.Count, offerId);
                Add(clicks);
            });
            End();
        }
    }
}
