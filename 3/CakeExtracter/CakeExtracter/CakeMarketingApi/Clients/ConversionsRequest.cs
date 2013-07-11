using System;

namespace CakeExtracter.CakeMarketingApi.Clients
{
    public class ConversionsRequest : ApiRequest
    {
        public ConversionsRequest()
        {
            start_date = DateTime.Today.ToString("MM/dd/yyyy");
            end_date = DateTime.Today.AddDays(1).ToString("MM/dd/yyyy");
            include_tests = "FALSE";
            start_at_row = 1;
            row_limit = 0;
            sort_field = "conversion_id";
            sort_descending = "FALSE";
        }

        //start_date / DATETIME = Report Start Date [MM/DD/YYYY HH:MM:SS]
        public string start_date { get; set; }

        //end_date / DATETIME = Report End Date [MM/DD/YYYY HH:MM:SS]
        public string end_date { get; set; }

        //affiliate_id / INT = Affiliate ID ["0" = ALL Affiliates] {See export.asmx > Affiliates}
        public int affiliate_id { get; set; }

        //advertiser_id / INT = Advertiser ID ["0" = ALL Advertisers] {See export.asmx > Advertisers}
        public int advertiser_id { get; set; }

        //offer_id / INT = Offer ID ["0" = ALL Offers] {See export.asmx > Offers}
        public int offer_id { get; set; }

        //campaign_id / INT = Campaign ID ["0" = ALL Campaigns] {See export.asmx > Campaigns}
        public int campaign_id { get; set; }

        //creative_id / INT = Creative ID ["0" = ALL Creatives] {See export.asmx > Creatives}
        public int creative_id { get; set; }

        //include_tests / BOOL = Include Test Clicks? ["TRUE", "FALSE"]
        public string include_tests { get; set; }

        //start_at_row / INT = Starting Row Number [Usually "1", unless doing incremental API Calls]
        public int start_at_row { get; set; }

        //row_limit / INT = Maximum Rows Returned ["0" = ALL Rows, "100000" = Maximum]
        public int row_limit { get; set; }

        //sort_field / STRING = Sort Field ["conversion_id", "visitor_id", "request_session_id", "click_id", "conversion_date", "transaction_id", "last_updated"]
        public string sort_field { get; set; }

        //sort_descending / BOOL = Sort Descending? ["TRUE", "FALSE"]
        public string sort_descending { get; set; }
    }
}