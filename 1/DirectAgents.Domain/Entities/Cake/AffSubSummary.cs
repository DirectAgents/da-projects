using System;

namespace DirectAgents.Domain.Entities.Cake
{
    public class AffSubSummary
    {
        public int AffSubId { get; set; }
        public virtual AffSub AffSub { get; set; }
        public int OfferId { get; set; }
        public virtual Offer Offer { get; set; }
        public DateTime Date { get; set; }

        public int Views { get; set; }
        public int Clicks { get; set; }
        public decimal Conversions { get; set; }
        public decimal Paid { get; set; }
        public decimal Sellable { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
    }
}
