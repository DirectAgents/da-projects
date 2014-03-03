using CakeExtracter.CakeMarketingApi;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.CakeMarketing.Extracters
{
    public class DailySummariesExtracter : Extracter<OfferDailySummary>
    {
        private readonly DateRange dateRange;
        private readonly int advertiserId;
        private readonly int? offerId;

        public DailySummariesExtracter(DateRange dateRange, int advertiserId, int? offerId)
        {
            this.dateRange = dateRange;
            this.advertiserId = advertiserId;
            this.offerId = offerId;
        }

        protected override void Extract()
        {
            Logger.Info("Getting offerIds for advertiserId={0}", advertiserId);

            List<int> offerIds;
            if (this.offerId.HasValue)
            {
                offerIds = new List<int> { offerId.Value };
            }
            else
            {
                offerIds = CakeMarketingUtility.OfferIds(advertiserId);
            }
    
            Logger.Info("Extracting DailySummaries for {0} offerIds between {2} and {3}: {1}",
                            offerIds.Count,
                            string.Join(", ", offerIds),
                            dateRange.FromDate.ToShortDateString(),
                            dateRange.ToDate.ToShortDateString());

            Parallel.ForEach(offerIds, offerId =>
            {
                var dailySummaries = CakeMarketingUtility.DailySummaries(dateRange, advertiserId, offerId, 0);

                Logger.Info("Extracted {0} DailySummaries for offerId={1}", dailySummaries.Count, offerId);

                Add(dailySummaries.Select(c => new OfferDailySummary(offerId, c)));

                // Check for dates with no data
                var datesExtracted = dailySummaries.Select(ds => ds.Date);
                for (var iDate = dateRange.FromDate; iDate < dateRange.ToDate; iDate = iDate.AddDays(1))
                {
                    if (!datesExtracted.Contains(iDate))
                        Add(new OfferDailySummary(offerId, iDate));
                }
            });

            End();
        }
    }
}
