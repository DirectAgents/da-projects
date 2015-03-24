using System;
using System.Collections.Generic;

namespace AdRoll.Entities
{
    public class AdSummaryReportResponse
    {
        public List<AdSummary> results { get; set; }
    }

    public class AdSummary
    {
        public DateTime date { get; set; } // not returned from the api

        public string eid { get; set; }
        public string ad { get; set; } // ad name
        public int height { get; set; }
        public int width { get; set; }
        public DateTime created_date { get; set; }
        public string type { get; set; }

        public double cost_USD { get; set; }
        public int impressions { get; set; }
        // paid_impressions
        public int total_conversions { get; set; }
        // adjusted_total_conversions
        public int clicks { get; set; }

        public bool AllZeros()
        {
            return (impressions == 0 && clicks == 0 && total_conversions == 0 && cost_USD == 0);
        }
    }
}
