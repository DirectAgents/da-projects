using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.CakeMarketingApi.Clients
{
    public class CampaignSummariesRequest : ApiRequest
    {
        public CampaignSummariesRequest()
        {
            start_date = DateTime.Today.ToString("MM/dd/yyyy");
            end_date = DateTime.Today.AddDays(1).ToString("MM/dd/yyyy");
            revenue_filter = "conversions_and_events";
        }

        public string start_date { get; set; }
        public string end_date { get; set; }
        public int affiliate_id { get; set; }
        public int affiliate_tag_id { get; set; }
        public int offer_id { get; set; }
        public int offer_tag_id { get; set; }
        public int campaign_id { get; set; }
        public int event_id { get; set; }
        public string revenue_filter { get; set; } // "conversions_and_events" or "conversions" or "events"
    }
}
