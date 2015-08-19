using System;

namespace AdRoll.Entities
{
    public class StatSummary
    {
        public int impressions { get; set; }
        // paid_impressions
        public int click_through_conversions { get; set; }
        public int view_through_conversions { get; set; }
        public int total_conversions { get; set; } // TODO: remove this - if don't need it for AdDailySummaries anymore
        // adjusted verions... ctc, vtc, total_converions
        public int clicks { get; set; }
        public double cost_USD { get; set; }
        public int prospects { get; set; }

        public bool AllZeros()
        {
            return (impressions == 0 && clicks == 0 && click_through_conversions == 0 && view_through_conversions == 0 && total_conversions == 0 && cost_USD == 0 && prospects == 0);
        }
    }

    // used for Advertisable daily report
    public class AdrollDailySummary : StatSummary
    {
        public DateTime date { get; set; } // not always returned from the api
    }

    // used for Ad daily report
    public class AdSummary : AdrollDailySummary
    {
        public string eid { get; set; }
        public string ad { get; set; } // ad name
        public int height { get; set; }
        public int width { get; set; }
        public DateTime created_date { get; set; }
        public string type { get; set; }
    }
}
