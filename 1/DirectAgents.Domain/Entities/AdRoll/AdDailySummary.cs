using System;

namespace DirectAgents.Domain.Entities.AdRoll
{
    public class AdDailySummary
    {
        public DateTime Date { get; set; }
        public int AdId { get; set; }
        public virtual Ad Ad { get; set; }

        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int CTC { get; set; }
        public int VTC { get; set; }
        public decimal Cost { get; set; }
        public int Prospects { get; set; }
    }
}
