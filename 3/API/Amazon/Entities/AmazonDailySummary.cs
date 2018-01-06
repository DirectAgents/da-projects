using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Entities
{
    public class StatSummary
    {
        public int impressions { get; set; }
        // paid_impressions
        public int click_through_conversions { get; set; }
        public int view_through_conversions { get; set; }
        public int attributedConversions30d { get; set; }
        public int clicks { get; set; }
        public decimal cost { get; set; }
        public int prospects { get; set; }

        public virtual bool AllZeros(bool includeProspects = false)
        {
            bool allZeros = (impressions == 0 && clicks == 0 && click_through_conversions == 0 && view_through_conversions == 0 && cost == 0);
            if (includeProspects)
                return (allZeros && prospects == 0);
            else
                return allZeros;
        }
    }

    // used for Advertisable daily report
    public class AmazonDailySummary : StatSummary
    {
        public Int64 campaignId { get; set; }
        public DateTime date { get; set; } 
    }
    public class AmazonAdDailySummary : StatSummary
    {
        public string adId { get; set; }
        public DateTime date { get; set; }
    }

    // used for Campaign daily report
    public class AmazonCampaignSummary : AmazonCampaign
    {
        public string eid { get; set; }
        public string campaign { get; set; } // campaign name
        public string advertiser { get; set; } // advertisable name
        public string type { get; set; } // e.g. "Retargeting"
        public string status { get; set; } // e.g. "approved"
        public DateTime created_date { get; set; }
        public DateTime start_date { get; set; }
        public DateTime? end_date { get; set; }
        public double budget_USD { get; set; }
        public double adjusted_attributed_click_through_rev { get; set; }
        public double adjusted_attributed_view_through_rev { get; set; }

        public bool AllZeros(bool includeProspects = false)
        {
            return AllZeros(includeProspects: includeProspects) && adjusted_attributed_click_through_rev == 0 && adjusted_attributed_view_through_rev == 0;
        }
    }

}
