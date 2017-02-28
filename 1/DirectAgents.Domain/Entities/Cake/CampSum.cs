using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.Cake
{
    public class CampSum
    {
        public int CampId { get; set; }
        public DateTime Date { get; set; }

        public int OfferId { get; set; } //TODO: index/FK
        public int AffId { get; set; }   //TODO: index/FK

        public int Views { get; set; }
        public int Clicks { get; set; }
        public decimal Conversions { get; set; }
        public decimal Paid { get; set; }
        public decimal Sellable { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }

        //RevCurr, CostCurr, PriceFormat

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal RevenuePerUnit { get; private set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal CostPerUnit { get; private set; }
    }
}
