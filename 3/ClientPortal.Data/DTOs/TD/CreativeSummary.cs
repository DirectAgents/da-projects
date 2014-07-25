using System;

namespace ClientPortal.Data.DTOs.TD
{
    public class CreativeSummary
    {
        public int CreativeID { get; set; }
        public string CreativeName { get; set; }

        //public string Currency { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public decimal Spend { get; set; }

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
            get { return (Impressions == 0) ? 0 : 1000 * Spend / Impressions; }
        }
        public decimal CPC
        {
            get { return (Clicks == 0) ? 0 : Spend / Clicks; }
        }
        public decimal CPA
        {
            get { return (Conversions == 0) ? 0 : Spend / Conversions; }
        }

        public int Conv // used for sorting
        {
            get { return (Conversions > 0) ? 1 : 0; }
        }
    }
}
