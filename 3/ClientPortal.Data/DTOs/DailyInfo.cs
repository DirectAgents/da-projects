using System;

namespace ClientPortal.Data.DTOs
{
    public class DailyInfo
    {
        public string Id { get { return Date.ToString("yyyyMMdd"); } }
        public DateTime Date { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public float ConversionPct
        {
            get { return (Clicks == 0) ? 0 : (float)Math.Round((double)Conversions / Clicks, 3); }
        }
        public decimal Revenue { get; set; }
        public decimal EPC
        {
            get { return (Clicks == 0) ? 0 : Math.Round(Revenue / Clicks, 2); }
        }
        public string Currency
        {
            set { Culture = OfferInfo.CurrencyToCulture(value); }
        }
        public string Culture { get; set; }
    }
}
