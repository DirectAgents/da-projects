using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.TD
{
    public class DailySummary
    {
        public DateTime Date { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }

        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int PostClickConv { get; set; }
        public int PostViewConv { get; set; }
        public decimal Cost { get; set; }

        //TotalConv

        public bool AllZeros()
        {
            return (Impressions == 0 && Clicks == 0 && PostClickConv == 0 && PostViewConv == 0 && Cost == 0);
        }
    }
}
