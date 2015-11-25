using System;

namespace FacebookAPI.Entities
{
    public class FBSummary
    {
        public DateTime Date { get; set; }
        public decimal Spend { get; set; }
        public int Impressions { get; set; }
        public int UniqueClicks { get; set; }
        public int TotalActions { get; set; }

        public bool AllZeros()
        {
            return (Spend == 0 && Impressions == 0 && UniqueClicks == 0 && TotalActions == 0);
        }
    }
}
