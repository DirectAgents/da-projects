using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class StatsSummary
    {
        public int Views { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public int Paid { get; set; }
        public int Sellable { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
    }
}
