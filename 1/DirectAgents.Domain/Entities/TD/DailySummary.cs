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
        public int PostClickConv { get; set; }
        public int PostViewConv { get; set; }
        public decimal Cost { get; set; }

        //TotalConv
    }

    // DTO
    public class TDStatWithAccount : TDStat
    {
        public Account Account { get; set; }
        public string AccountName
        {
            get { return (Account != null) ? Account.Name : string.Empty; }
        }
        public string PlatformName
        {
            get { return (Account != null && Account.Platform != null) ? Account.Platform.Name : string.Empty; }
        }
    }

    public class TDStat
    {
        public string Name { get; set; }

        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int PostClickConv { get; set; }
        public int PostViewConv { get; set; }
        public decimal Cost { get; set; }
        //public int Prospects { get; set; }

        public int TotalConv
        {
            get { return PostClickConv + PostViewConv; }
        }

        public bool AllZeros()
        {
            return (Impressions == 0 && Clicks == 0 && PostClickConv == 0 && PostViewConv == 0 && Cost == 0);
        }

        // Computed properties
        public double CTR
        {
            get { return (Impressions == 0) ? 0 : Math.Round((double)Clicks / Impressions, 4); }
        }
        public double ConvRate
        {
            get { return (Clicks == 0) ? 0 : Math.Round((double)TotalConv / Clicks, 4); }
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
            get { return (TotalConv == 0) ? 0 : Math.Round(Cost / TotalConv, 2); }
        }
    }
}
