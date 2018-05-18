using System;
using System.Collections.Generic;
using System.Linq;
using DAgents.Common;

namespace EomApp1.Screens.Synch.Services.Cake
{
    public class CakeWebService
    {
        private static readonly string ApiKey = "FCjdYAcwQE";
        private ILogger logger;
        private CakeWebServiceCache cache;

        public CakeWebService(ILogger logger)
        {
            this.logger = logger;
            this.cache = new CakeWebServiceCache();
        }

        public EomApp1.Cake.WebServices._3.Export.offer1 OfferById(int offerID)
        {
            EomApp1.Cake.WebServices._3.Export.offer1 offer;

            if (this.cache.ContainsOfferById(offerID))
            {
                this.logger.Log(string.Format("Using cached offer information for offerID={0}.", offerID));

                offer = cache.OfferById(offerID);
            }
            else
            {
                this.logger.Log(string.Format("Calling Cake web service to get offer information for offerID={0}.", offerID));

                var service = GetExportServiceV3();

                var response = service.Offers(
                                        api_key: ApiKey,
                                        offer_id: offerID,
                                        offer_name: null,
                                        advertiser_id: 0,
                                        vertical_id: 0,
                                        offer_type_id: 0,
                                        media_type_id: 0,
                                        offer_status_id: 0,
                                        tag_id: 0,
                                        start_at_row: 0,
                                        row_limit: 0,
                                        sort_field: EomApp1.Cake.WebServices._3.Export.OffersSortFields.offer_id,
                                        sort_descending: false);

                if (response.success)
                {
                    offer = response.offers[0];
                    this.cache.OfferById(offerID, offer);
                }
                else
                {
                    throw new Exception("Error exporting affiliates: " + response.message);
                }
            }

            return offer;
        }

        public EomApp1.Cake.WebServices._4.Export.advertiser1 AdvertiserById(int advertiserID)
        {
            EomApp1.Cake.WebServices._4.Export.advertiser1 advertiser = null;

            if (this.cache.ContainsAdvertiserById(advertiserID))
            {
                this.logger.Log(string.Format("Using cached advertiser information for advertiserID={0}.", advertiserID));

                advertiser = cache.AdvertiserById(advertiserID);
            }
            else
            {
                this.logger.Log(string.Format("Calling Cake web service to get advertiser information for advertiserID={0}.", advertiserID));

                var service = GetExportServiceV4();

                var response = service.Advertisers(
                                        api_key: ApiKey,
                                        advertiser_id: advertiserID,
                                        advertiser_name: null,
                                        account_manager_id: 0,
                                        tag_id: 0,
                                        start_at_row: 0,
                                        row_limit: 0,
                                        sort_field: EomApp1.Cake.WebServices._4.Export.AdvertisersSortFields.advertiser_id,
                                        sort_descending: false);

                if (response.success)
                {
                    advertiser = response.advertisers[0];
                    this.cache.AdvertiserById(advertiserID, advertiser);
                }
                else
                {
                    throw new Exception("Error exporting advertisers: " + response.message);
                }
            }

            return advertiser;
        }

        public EomApp1.Cake.WebServices._4.Export.affiliate AffiliateById(int affiliateID)
        {
            EomApp1.Cake.WebServices._4.Export.affiliate affiliate = null;

            if (this.cache.ContainsAffiliateById(affiliateID))
            {
                this.logger.Log(string.Format("Using cached affiliate information for affiliateID={0}.", affiliateID));

                affiliate = cache.AffiliateById(affiliateID);
            }
            else
            {
                this.logger.Log(string.Format("Calling Cake web service to get affiliate information for affiliateID={0}.", affiliateID));

                var service = GetExportServiceV4();

                var response = service.Affiliates(
                                    api_key: ApiKey,
                                    affiliate_id: affiliateID,
                                    affiliate_name: null,
                                    account_manager_id: 0,
                                    tag_id: 0,
                                    start_at_row: 0,
                                    row_limit: 0,
                                    sort_field: EomApp1.Cake.WebServices._4.Export.AffiliatesSortFields.affiliate_id,
                                    sort_descending: false);

                if (response.success)
                {
                    affiliate = response.affiliates[0];
                    cache.AffiliateById(affiliateID, affiliate);
                }
                else
                {
                    throw new Exception("Error exporting affiliate: " + response.message);
                }
            }

            return affiliate;
        }

        public List<EomApp1.Cake.WebServices._4.Reports.conversion> Conversions(int offerID, int? affiliateID, DateTime fromDate, DateTime toDate)
        {
            // Cake treats the to-date as exclusive, so we go one day past it
            toDate = toDate.AddDays(1);

            if (!affiliateID.HasValue)
                affiliateID = 0;

            var result = new List<EomApp1.Cake.WebServices._4.Reports.conversion>();
            bool success = false;

            try
            {
                result.AddRange(ConversionsLocal(offerID, fromDate, toDate, affiliateID.Value));
                success = true;
            }
            catch (Exception ex)
            {
                // Rethrow unless timeout
                if (!ex.Message.Contains("timeout"))
                    throw;
            }

            if (!success) // timeout, try splitting date range
            {
                const int daysAtATime = 3;

                logger.Log("Timeout occured, trying again " + daysAtATime + " days at a time..");

                result.Clear();
                foreach (var dateRange in SplitDateRange(fromDate, toDate, daysAtATime))
                {
                    logger.Log("Current date range is " + dateRange.Item1.ToShortDateString() + " to " + dateRange.Item2.ToShortDateString());

                    result.AddRange(ConversionsLocal(offerID, dateRange.Item1, dateRange.Item2, affiliateID.Value));
                }
            }

            return result;
        }

        public static IEnumerable<Tuple<DateTime, DateTime>> SplitDateRange(DateTime start, DateTime end, int dayChunkSize)
        {
            DateTime chunkEnd;

            while ((chunkEnd = start.AddDays(dayChunkSize)) < end)
            {
                yield return Tuple.Create(start, chunkEnd);
                start = chunkEnd;
            }

            yield return Tuple.Create(start, end);
        }

        private List<EomApp1.Cake.WebServices._4.Reports.conversion> ConversionsLocal(int offerID, DateTime fromDate, DateTime toDate, int affiliateID = 0)
        {
            const int batchSize = 5000;
            var client = GetReportsServiceV4();

            Func<int, EomApp1.Cake.WebServices._4.Reports.conversion_report_response> getResponse = (start) =>
            {
                logger.Log("Extracting conversions, starting at row " + start);

                var resp = client.Conversions(api_key: ApiKey, start_date: fromDate, end_date: toDate,
                                              affiliate_id: affiliateID, advertiser_id: 0, offer_id: offerID, campaign_id: 0,
                                              creative_id: 0, include_tests: false, start_at_row: start, row_limit: batchSize,
                                              sort_field: EomApp1.Cake.WebServices._4.Reports.ConversionsSortFields.conversion_id,
                                              sort_descending: false);

                if (!resp.success)
                    throw new Exception("conversions client failed");

                return resp;
            };

            EomApp1.Cake.WebServices._4.Reports.conversion_report_response response;
            var result = new List<EomApp1.Cake.WebServices._4.Reports.conversion>();
            int startRow = 1;

            // testing
            List<string> idList = new List<string>();
            do
            {
                response = getResponse(startRow);
                //result.AddRange(response.conversions.ToList());
                var convList = response.conversions.ToList();
                foreach (var conv in convList)
                {
                    if (!idList.Contains(conv.conversion_id))
                    {
                        result.Add(conv);
                        idList.Add(conv.conversion_id);
                    }
                }
                startRow += batchSize;
            }
            while (response.conversions.Length == batchSize);

            return result;
        }

        private static EomApp1.Cake.WebServices._3.Export.exportSoap GetExportServiceV3()
        {
            var service = new EomApp1.Cake.WebServices._3.Export.exportSoapClient("exportSoap1");
            return service;
        }

        private static EomApp1.Cake.WebServices._4.Export.exportSoap GetExportServiceV4()
        {
            var service = new EomApp1.Cake.WebServices._4.Export.exportSoapClient("exportSoap");
            return service;
        }

        private static EomApp1.Cake.WebServices._4.Reports.reportsSoap GetReportsServiceV4()
        {
            var service = new EomApp1.Cake.WebServices._4.Reports.reportsSoapClient("reportsSoap");
            return service;
        }
    }
}
