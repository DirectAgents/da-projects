using System;
using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.CakeMarketingApi.Clients
{
    public class SubIdSummariesClient : ApiClient
    {
        public SubIdSummariesClient()
            : base(1, "reports", "SubIDSummary")
        {
        }

        public SubIdSummaryResponse SubIdSummaries(SubIdSummariesRequest request)
        {
            var result = Execute<SubIdSummaryResponse>(request);
            return result;
        }

    }

    public class SubIdSummariesRequest : ApiRequest
    {
        public SubIdSummariesRequest()
        {
            start_date = DateTime.Today.ToString("MM/dd/yyyy");
            end_date = DateTime.Today.AddDays(1).ToString("MM/dd/yyyy");
            revenue_filter = "conversions_and_events";
        }

        //Note: max daterange is 31 days
        public string start_date { get; set; }
        public string end_date { get; set; }
        public int source_affiliate_id { get; set; }
        public int site_offer_id { get; set; }
        public int event_id { get; set; }
        public string revenue_filter { get; set; } // "conversions_and_events" or "conversions" or "events"
    }
}
