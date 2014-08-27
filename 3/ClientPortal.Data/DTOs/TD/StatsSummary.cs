using System;

namespace ClientPortal.Data.DTOs.TD
{
    public class StatsSummary
    {
        public DateTime Date { get; set; }

        //public string Currency { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public decimal Spend { get; set; }

        // Computed properties
        public double CTR
        {
            get { return (Impressions == 0) ? 0 : Math.Round((double)Clicks / Impressions, 4); }
        }
        public double ConvRate
        {
            get { return (Clicks == 0) ? 0 : Math.Round((double)Conversions / Clicks, 4); }
        }

        public decimal CPM
        {
            get { return (Impressions == 0) ? 0 : Math.Round(1000 * Spend / Impressions, 3); }
        }
        public decimal CPC
        {
            get { return (Clicks == 0) ? 0 : Math.Round(Spend / Clicks, 3); }
        }
        public decimal CPA
        {
            get { return (Conversions == 0) ? 0 : Math.Round(Spend / Conversions, 3); }
        }
    }
}
