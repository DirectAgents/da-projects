using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientPortal.Data.Entities.TD
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
