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
}
