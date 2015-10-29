
namespace DirectAgents.Domain.DTO
{
    public interface ITDLineItem : ITDRawLineItem
    {
        ITDClickStats Stats { get; }

        decimal CPM { get; }
        decimal CPC { get; }
        decimal CPA { get; }
    }
    public interface ITDRawLineItem
    {
        // CopyFrom(), AllZeros() ?

        decimal DACost { get; }
        decimal ClientCost { get; } // may or may not go through us
        decimal TotalRevenue { get; }
        decimal MgmtFee { get; }
        decimal Margin { get; }
        decimal? MarginPct { get; }
    }
    public interface ITDClickStats
    {
        int Impressions { get; }
        int Clicks { get; }
        int PostClickConv { get; }
        int PostViewConv { get; }
        int TotalConv { get; }

        double CTR { get; }
        double ConvRate { get; }
    }

    public class TDRawLineItem : ITDRawLineItem
    {
        public virtual void CopyFrom(TDRawLineItem stat)
        {
            this.DACost = stat.DACost;
            this.ClientCost = stat.ClientCost;
            this.TotalRevenue = stat.TotalRevenue;
            this.CostGoesThruDA = stat.CostGoesThruDA;
        }
        public virtual bool AllZeros(bool includeCostThatDoesntGoThruDA = true)
        {
            if (includeCostThatDoesntGoThruDA)
                return (DACost == 0 && ClientCost == 0 && TotalRevenue == 0);
            else
                return DACost == 0 && TotalRevenue == 0 && (ClientCost == 0 || !CostGoesThruDA);
        }

        public decimal DACost { get; set; }
        public decimal ClientCost { get; set; } // may or may not go through us
        //public decimal MediaSpend { get; set; } // may or may not go through us
        public decimal TotalRevenue { get; set; }
        public bool CostGoesThruDA = true;

        public decimal RawCost
        {
            set { SetMoneyVals(value); }
        }

        // Constructor
        public TDRawLineItem(decimal rawCost = 0, decimal mgmtFeePct = 0, decimal marginPct = 0)
        {
            SetMoneyVals(rawCost, mgmtFeePct, marginPct);
        }

        public decimal MgmtFee
        {
            get { return (CostGoesThruDA ? TotalRevenue - ClientCost : TotalRevenue); }
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
                ClientCost = rawCost;
                TotalRevenue = rawCost * mgmtFeePct / 100;
            }
            else
            {
                CostGoesThruDA = true;
                DACost = rawCost;
                TotalRevenue = rawCost / RevToCostMultiplier(marginPct);
                ClientCost = TotalRevenue / ClientCostToRevMultiplier(mgmtFeePct);
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
