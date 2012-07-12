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
            this.cache = new CakeWebServiceCache(logger);
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

        public List<EomApp1.Cake.WebServices._4.Reports.conversion> Conversions(int offerID, DateTime fromDate, DateTime toDate)
        {
            this.logger.Log(string.Format("Calling Cake web service to get Conversions for OfferID={0} from {1} to {2}.", offerID, fromDate, toDate));

            var client = GetReportsServiceV4();

            var response = client.Conversions(
                                        api_key: ApiKey,
                                        start_date: fromDate,
                                        end_date: toDate.AddDays(1),
                                        affiliate_id: 0,
                                        advertiser_id: 0,
                                        offer_id: offerID,
                                        campaign_id: 0,
                                        creative_id: 0,
                                        include_tests: false,
                                        start_at_row: 0,
                                        row_limit: 0,
                                        sort_field: EomApp1.Cake.WebServices._4.Reports.ConversionsSortFields.conversion_id,
                                        sort_descending: false);

            if (!response.success)
            {
                throw new Exception(response.message);
            }
            else
            {
                var conversions = response.conversions;

                this.logger.Log(string.Format("Got {0} conversions.", conversions.Length));

                return conversions.ToList();
            }
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
