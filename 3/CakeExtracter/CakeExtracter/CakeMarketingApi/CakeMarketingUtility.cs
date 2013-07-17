using System;
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

        public static List<Click> Clicks(DateRange dateRange, int advertiserId, int offerId, out int rowCount)
        {
            var request = new ClicksRequest
            {
                start_date = dateRange.FromDate.ToString("MM/dd/yyyy"),
                end_date = dateRange.ToDate.ToString("MM/dd/yyyy"),
                advertiser_id = advertiserId,
                offer_id = offerId
            };
            var client = new ClicksClient();
            var response = client.Clicks(request);
            if (!response.Success)
                throw new Exception("ClicksClient failed");        
            rowCount = response.RowCount;
            return response.Clicks.ToList();
        }

        public static List<Conversion> Conversions(DateRange dateRange, int advertiserId, int offerId)
        {
            var request = new ConversionsRequest
            {
                start_date = dateRange.FromDate.ToString("MM/dd/yyyy"),
                end_date = dateRange.ToDate.ToString("MM/dd/yyyy"),
                advertiser_id = advertiserId,
                offer_id = offerId
            };
            var client = new ConversionsClient();
            var response = client.Conversions(request);
            return response.Conversions.ToList();
        }

        public static List<Traffic> Traffic(DateRange dateRange)
        {
            var request = new TrafficRequest
            {
                start_date = dateRange.FromDate.ToString("MM/dd/yyyy"),
                end_date = dateRange.ToDate.ToString("MM/dd/yyyy"),
            };
            var client = new TrafficClient();
            var response = client.Traffic(request);
            return response.Traffics.ToList();
        }
    }
}