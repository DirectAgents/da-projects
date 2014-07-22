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
    }
}
