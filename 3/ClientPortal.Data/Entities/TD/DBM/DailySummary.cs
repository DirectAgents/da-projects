using System;

namespace ClientPortal.Data.Entities.TD.DBM
{
    public class DailySummary
    {
        public DateTime Date { get; set; }
        public int InsertionOrderID { get; set; }
        public virtual InsertionOrder InsertionOrder { get; set; }

        public string AdvertiserCurrency { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public decimal Revenue { get; set; }
    }
}
