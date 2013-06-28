using System.Collections.Generic;
using System.Linq;
using CakeExtracter.CakeMarketingApi.Clients;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;

namespace CakeExtracter.CakeMarketingApi
{
    public static class CakeMarketingUtility
    {
        public static List<int> OfferIds(int advertiserId)
        {
            var client = new OffersClient();
            var request = new OffersRequest
                {
                    advertiser_id = advertiserId
                };
            var response = client.Offers(request);
            var offerIds = response.Offers.Select(c => c.OfferId);
            return offerIds.ToList();
        }

        public static List<DailySummary> DailySummaries(DateRange dateRange, int advertiserId, int offerId)
        {
            var client = new DailySummariesClient();
            var request = new DailySummariesRequest
            {
                advertiser_id = advertiserId,
                offer_id = offerId,
                start_date = dateRange.FromDate.ToString("MM/dd/yyyy"),
                end_date = dateRange.ToDate.ToString("MM/dd/yyyy")
            };
            var response = client.DailySummaries(request);
            return response.DailySummaries;
        }
    }
}