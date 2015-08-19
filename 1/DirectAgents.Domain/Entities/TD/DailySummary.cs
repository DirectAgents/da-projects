using System;

namespace DirectAgents.Domain.Entities.TD
{
    public class DailySummary
    {
        public DateTime Date { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }

        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        // CTC? VTC?
        public decimal Cost { get; set; }
    }

    // DTO
    public class TDStat
    {
        //public string Name { get; set; }
        public Account Account { get; set; }
        public string AccountName
        {
            get { return (Account != null) ? Account.Name : string.Empty; }
        }
        public string PlatformName
        {
            get { return (Account != null && Account.Platform != null) ? Account.Platform.Name : string.Empty; }
        }

        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public decimal Cost { get; set; }

        public bool AllZeros()
        {
            return (Impressions == 0 && Clicks == 0 && Conversions == 0 && Cost == 0);
        }

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
            get { return (Impressions == 0) ? 0 : Math.Round(1000 * Cost / Impressions, 2); }
        }
        public decimal CPC
        {
            get { return (Clicks == 0) ? 0 : Math.Round(Cost / Clicks, 2); }
        }
        public decimal CPA
        {
            get { return (Conversions == 0) ? 0 : Math.Round(Cost / Conversions, 2); }
        }
    }
}
