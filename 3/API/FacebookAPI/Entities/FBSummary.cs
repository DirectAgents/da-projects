using System;

namespace FacebookAPI.Entities
{
    public class FBSummary
    {
        public DateTime Date { get; set; }
        public decimal Spend { get; set; }
        public int Impressions { get; set; }
        public int UniqueClicks { get; set; }
        public int LinkClicks { get; set; }
        public int Conversions_28d_click { get; set; }
        public int Conversions_1d_view { get; set; }
        public int TotalActions { get; set; }

        public string CampaignId { get; set; }
        public string CampaignName { get; set; }
        public string AdSetId { get; set; }
        public string AdSetName { get; set; }
        public string AdId { get; set; }
        public string AdName { get; set; }

        public bool AllZeros()
        {
            return (Spend == 0 && Impressions == 0 && UniqueClicks == 0 && LinkClicks == 0 && Conversions_28d_click == 0 && Conversions_1d_view == 0 && TotalActions == 0);
        }
    }
}
