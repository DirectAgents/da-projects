using System;
using System.Collections.Generic;
using System.Linq;
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
            LogGettingOffers();
            var offerIds = CakeMarketingUtility.OfferIds(advertiserId);
            LogExtractingClicks(offerIds);
            foreach (var offerId in offerIds)
            {
                var clicks = 
                    RetryUtility.Retry(3, 10000, new[] { typeof(Exception) }, () =>
                        CakeMarketingUtility.EnumerateClicks(dateRange, advertiserId, offerId));
                
                foreach (var clickBatch in clicks.InBatches(1000))
                {
                    LogExtractedClicks(offerId, clickBatch);
                    Add(clickBatch);
                }
            }
            End();
        }

        private void LogExtractedClicks(int offerId, IEnumerable<Click> clicks)
        {
            Logger.Info("Extracted {0} Clicks for offerId={1}", clicks.Count(), offerId);
        }

        private void LogGettingOffers()
        {
            Logger.Info("Getting offerIds for advertiserId={0}", advertiserId);
        }

        private void LogExtractingClicks(List<int> offerIds)
        {
            Logger.Info("Extracting Clicks for {0} offerIds between {2} and {3}: {1}",
                offerIds.Count,
                string.Join(", ", offerIds),
                dateRange.FromDate.ToShortDateString(),
                dateRange.ToDate.ToShortDateString());
        }
    }
}
