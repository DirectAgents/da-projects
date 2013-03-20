using System;

namespace ClientPortal.Data.DTOs
{
    public class AdvertiserSummary
    {
        public int AdvertiserId { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public decimal Revenue { get; set; }

    }
}
