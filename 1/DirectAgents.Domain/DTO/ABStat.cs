using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.DTO
{
    //TODO? make an IABStat interface and change all referencing code to use that?

    // "Accounting Backup" stats - for the dashboard
    public class ABStat
    {
        private const int NUM_DECIMALS_FOR_ROUNDING = 5;

        //TODO: RTId ?  (make nullable...? Id? ABId?)

        public int Id { get; set; }

        public string Client { get; set; }
        //public string Campaign { get; set; }
        //public string Vendor { get; set; }

        public decimal Rev { get; set; }
        public decimal Cost { get; set; }
        public decimal Margin
        {
            get { return Rev - Cost; }
        }

        public decimal Budget { get; set; }
        public decimal OverBudget // (Amount over budget)
        {
            get { return Rev - Budget; }
        }
        public decimal FractionBudget // (Fraction of budget used)
        {
            get { return (Budget == 0) ? 0 : Decimal.Round(Rev / Budget, NUM_DECIMALS_FOR_ROUNDING); }
        }

        public decimal StartBal { get; set; }
        public decimal CurrBal
        {
            get { return StartBal - Rev; }
        }
        public decimal ExtCred { get; set; } // this one: usually larger
        public decimal IntCred { get; set; } // this one: provides cushion
    }
}
