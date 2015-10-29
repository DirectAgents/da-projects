﻿
namespace DirectAgents.Domain.DTO
{
    public class TDLineItem
    {
        public virtual void CopyFrom(TDLineItem stat)
        {
            this.DACost = stat.DACost;
            this.MediaSpend = stat.MediaSpend;
            this.TotalRevenue = stat.TotalRevenue;
            this.CostGoesThruDA = stat.CostGoesThruDA;
        }
        public virtual bool AllZeros(bool includeCostThatDoesntGoThruDA = true)
        {
            if (includeCostThatDoesntGoThruDA)
                return (DACost == 0 && MediaSpend == 0 && TotalRevenue == 0);
            else
                return DACost == 0 && TotalRevenue == 0 && (MediaSpend == 0 || !CostGoesThruDA);
        }

        public decimal DACost { get; set; }
        //public decimal ClientCost { get; set; } // may or may not go through us
        public decimal MediaSpend { get; set; } // may or may not go through us
        public decimal TotalRevenue { get; set; }
        public bool CostGoesThruDA = true;

        public decimal RawCost
        {
            set { SetMoneyVals(value); }
        }

        // Constructor
        public TDLineItem(decimal rawCost = 0, decimal mgmtFeePct = 0, decimal marginPct = 0)
        {
            SetMoneyVals(rawCost, mgmtFeePct, marginPct);
        }

        public decimal MgmtFee()
        {
            return (CostGoesThruDA ? TotalRevenue - MediaSpend : TotalRevenue);
        }

        public decimal Margin
        {
            get { return (TotalRevenue - DACost); }
        }
        public decimal? MarginPct
        {
            get { return (TotalRevenue == 0) ? (decimal?)null : (100 * Margin / TotalRevenue); }
        }

        public void SetMoneyVals(decimal rawCost, decimal mgmtFeePct = 0, decimal marginPct = 0)
        {
            if (marginPct == 100)
            {
                CostGoesThruDA = false;
                DACost = 0;
                MediaSpend = rawCost;
                TotalRevenue = rawCost * mgmtFeePct / 100;
            }
            else
            {
                CostGoesThruDA = true;
                DACost = rawCost;
                TotalRevenue = rawCost / RevToCostMultiplier(marginPct);
                MediaSpend = TotalRevenue / ClientCostToRevMultiplier(mgmtFeePct);
            }
        }
        private decimal RevToCostMultiplier(decimal marginPct)
        {
            return (1 - marginPct / 100);
        }
        private decimal ClientCostToRevMultiplier(decimal mgmtFeePct)
        {
            return (1 + mgmtFeePct / 100);
        }
    }
}
