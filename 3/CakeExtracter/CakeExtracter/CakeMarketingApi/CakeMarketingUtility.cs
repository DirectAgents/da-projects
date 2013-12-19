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
        public static List<Advertiser> Advertisers(int advertiserId = 0)
        {
            var client = new AdvertisersClient();
            var request = new AdvertisersRequest
            {
                advertiser_id = advertiserId
            };
            var response = client.Advertisers(request);
            if (response != null)
                return response.Advertisers;
            else
            {
                Logger.Error(new Exception("Could not retrieve Advertisers."));
                return new List<Advertiser>();
            }
        }

        public static List<int> OfferIds(int advertiserId)
        {
            var client = new OffersClient();
            var request = new OffersRequest
                {
                    advertiser_id = advertiserId
                };
            var response = client.Offers(request);
            if (response == null || response.Offers == null)
            {
                Logger.Info("Unable to retrieve offers. Trying again...");
                response = client.Offers(request);
            }
            var offerIds = response.Offers.Select(c => c.OfferId);
            return offerIds.ToList();
        }

        public static List<Offer> Offers()
        {
            var client = new OffersClient();
            var request = new OffersRequest();
            var response = client.Offers(request);
            return response.Offers;
        }

        public static List<Campaign> Campaigns(int offerId)
        {
            var client = new CampaignsClient();
            var request = new CampaignsRequest
            {
                offer_id = offerId
            };
            var response = client.Campaigns(request);
            return response.Campaigns;
        }

        public static List<Creative> Creatives(int offerId)
        {
            var client = new CreativesClient();
            var request = new CreativesRequest
            {
                offer_id = offerId
            };
            var response = client.Creatives(request);
            return response.Creatives;
        }

        public static List<DailySummary> DailySummaries(DateRange dateRange, int advertiserId, int offerId, int creativeId)
        {
            var client = new DailySummariesClient();
            var request = new DailySummariesRequest
            {
                advertiser_id = advertiserId,
                offer_id = offerId,
                creative_id = creativeId,
                start_date = dateRange.FromDate.ToString("MM/dd/yyyy"),
                end_date = dateRange.ToDate.ToString("MM/dd/yyyy")
            };
            var response = client.DailySummaries(request);
            return response.DailySummaries;
        }

        public static List<Click> Clicks(DateRange dateRange, int advertiserId, int offerId, out int rowCount)
        {
            int startAtRow = 1;
            int rowLimitForOneCall = 5000;
            List<Click> result = new List<Click>();
            while (true)
            {
                int total = 0;
                var request = new ClicksRequest
                {
                    start_date = dateRange.FromDate.ToString("MM/dd/yyyy"),
                    end_date = dateRange.ToDate.ToString("MM/dd/yyyy"),
                    advertiser_id = advertiserId,
                    offer_id = offerId,
                    row_limit = rowLimitForOneCall,
                    start_at_row = startAtRow
                };

                var client = new ClicksClient();
                var response = client.Clicks(request);

                if (!response.Success)
                    throw new Exception("ClicksClient failed");

                total += response.Clicks.Count;
                result.AddRange(response.Clicks);
                if (total >= response.RowCount)
                {
                    Logger.Info("Extracted a total of {0}, returning result..", total);
                    break;
                }
                startAtRow += rowLimitForOneCall;
                Logger.Info("Extracted a total of {0}, checking for more, starting at row {1}..", total, startAtRow);
            }
            rowCount = result.Count;
            return result;
        }

        public static IEnumerable<Click> EnumerateClicks(DateRange dateRange, int advertiserId, int offerId)
        {
            // initialize to start at the first row
            int startAtRow = 1;

            // hard code an upper limit for the max number of rows to be returned in one call
            int rowLimitForOneCall = 5000;

            bool done = false;
            int total = 0;
            while (!done)
            {
                // prepare the request
                var request = new ClicksRequest
                {
                    start_date = dateRange.FromDate.ToString("MM/dd/yyyy"),
                    end_date = dateRange.ToDate.ToString("MM/dd/yyyy"),
                    advertiser_id = advertiserId,
                    offer_id = offerId,
                    row_limit = rowLimitForOneCall,
                    start_at_row = startAtRow
                };

                // create the client, call the service and check the response
                var client = new ClicksClient();
                ClickReportResponse response;
                try
                {
                    response = client.Clicks(request);
                }
                catch (Exception)
                {
                    Logger.Warn("Caught an exception while extracting clicks, bailing out..");
                    yield break;
                }

                if (response == null)
                {
                    throw new Exception("Clicks client returned null response");
                }

                if (!response.Success)
                {
                    throw new Exception("ClicksClient failed");
                }

                // update the running total
                total += response.Clicks.Count;

                // return result
                foreach (var click in response.Clicks)
                {
                    yield return click;
                }

                if (total >= response.RowCount)
                    done = true;
                else
                {
                    startAtRow += rowLimitForOneCall;
                    Logger.Info("Extracted a total of {0} rows, checking for more, starting at row {1}..", total, startAtRow);
                }
            }

            Logger.Info("Extracted a total of {0}, done.", total);
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