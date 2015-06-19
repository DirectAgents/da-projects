using System;
using System.Collections.Generic;

namespace AdRoll.Entities
{
    public class AdSummaryReportResponse
    {
        public List<AdSummary> results { get; set; }
    }
    public class DailySummaryReportResponse
    {
        public List<DailySummary> results { get; set; }
    }

    public class StatSummary
    {
        public double cost_USD { get; set; }
        public int impressions { get; set; }
        // paid_impressions
        public int total_conversions { get; set; }
        // adjusted_total_conversions
        public int clicks { get; set; }
        //public int prospects { get; set; }

        public bool AllZeros()
        {
            return (impressions == 0 && clicks == 0 && total_conversions == 0 && cost_USD == 0);
            //return (impressions == 0 && clicks == 0 && total_conversions == 0 && prospects == 0 && cost_USD == 0);
        }
    }

    // used for Advertisable daily report
    public class DailySummary : StatSummary
    {
        public DateTime date { get; set; } // not always returned from the api
    }

    // used for Ad daily report
    public class AdSummary : DailySummary
    {
        public string eid { get; set; }
        public string ad { get; set; } // ad name
        public int height { get; set; }
        public int width { get; set; }
        public DateTime created_date { get; set; }
        public string type { get; set; }
    }
}
