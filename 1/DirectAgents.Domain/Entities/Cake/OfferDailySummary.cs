using System;

namespace DirectAgents.Domain.Entities.Cake
{
    //TODO: make this inherit from the class below?

    public class OfferDailySummary
    {
        public int OfferId { get; set; }
        public DateTime Date { get; set; }
        public int Views { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public int Paid { get; set; }
        public int Sellable { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
    }

    // DTO
    public class StatsSummary
    {
        public string Name { get; set; }

        public int Views { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public int Paid { get; set; }
        public int Sellable { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }

        public decimal Margin
        {
            get { return Revenue - Cost; }
        }
    }
}
