using System;

namespace DirectAgents.Domain.Entities.AdRoll
{
    public class AdvertisableStat
    {
        public DateTime Date { get; set; }
        public int AdvertisableId { get; set; }
        public virtual Advertisable Advertisable { get; set; }

        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int CTC { get; set; }
        public int VTC { get; set; }
        public decimal Cost { get; set; }
        public int Prospects { get; set; }
    }

    // DTO
    public class AdRollStat
    {
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int ClickThruConv { get; set; }
        public int ViewThruConv { get; set; }
        public int TotalConv
        {
            get { return ClickThruConv + ViewThruConv; }
        }
        public decimal Spend { get; set; }
        public int Prospects { get; set; }

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
            get { return (Impressions == 0) ? 0 : Math.Round(1000 * Spend / Impressions, 2); }
        }
        public decimal CPC
        {
            get { return (Clicks == 0) ? 0 : Math.Round(Spend / Clicks, 2); }
        }
        public decimal CPA
        {
            get { return (TotalConv == 0) ? 0 : Math.Round(Spend / TotalConv, 2); }
        }
    }
}
