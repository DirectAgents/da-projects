using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.DTO
{
    public class TDStatWithBudget : TDStat
    {
        //public DateTime Date { get; set; } // the month of the budget
        public TDBudget Budget;

        public IEnumerable<TDStat> ExtAccountStats { get; set; }

        public TDStatWithBudget(IEnumerable<DailySummary> dSums, BudgetInfoVals budgetVals)
            : base(dSums, budgetVals)
        {
            Budget.MediaSpend = budgetVals.MediaSpend;
        }

        public TDStatWithBudget(TDStat tdStat, BudgetInfo budgetInfo)
        {
            CopyFrom(tdStat);
            Budget.MediaSpend = budgetInfo.MediaSpend;
        }

        public decimal FractionReached()
        {
            if (Budget.MediaSpend == 0)
                return 0;
            return this.MediaSpend() / Budget.MediaSpend;
        }

    }

    public class TDStat : TDMoneyStat
    {
        // Possible ways to identify the stats...
        public string Name { get; set; }
        public Campaign Campaign { get; set; }
        public ExtAccount ExtAccount { get; set; }
        public Platform Platform { get; set; }

        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int PostClickConv { get; set; }
        public int PostViewConv { get; set; }
        //public decimal Cost { get; set; } // (inherited)
        //public int Prospects { get; set; }

        public int TotalConv
        {
            get { return PostClickConv + PostViewConv; }
        }

        public override bool AllZeros()
        {
            bool moneyAllZeros = base.AllZeros();
            return (Impressions == 0 && Clicks == 0 && PostClickConv == 0 && PostViewConv == 0 && moneyAllZeros);
        }

        // Constructors
        public TDStat() { } // calls default base constructor too
        public TDStat(IEnumerable<DailySummary> dSums, MarginFeeVals marginFeeVals)
        {
            SetStatsAndMoneyValsFrom(dSums, marginFeeVals);
        }

        public override void CopyFrom(TDMoneyStat stat)
        {
            base.CopyFrom(stat); // copy money vals

            if (stat is TDStat)
            {
                TDStat tdStat = (TDStat)stat;
                this.Name = tdStat.Name;
                this.Campaign = tdStat.Campaign;
                this.ExtAccount = tdStat.ExtAccount;
                this.Platform = tdStat.Platform;

                this.Impressions = tdStat.Impressions;
                this.Clicks = tdStat.Clicks;
                this.PostClickConv = tdStat.PostClickConv;
                this.PostViewConv = tdStat.PostViewConv;
            }
        }

        private void SetStatsAndMoneyValsFrom(IEnumerable<DailySummary> dSums, MarginFeeVals marginFeeVals, bool roundCost = false)
        {
            decimal cost = 0;
            decimal mgmtFeePct = 0;
            decimal marginPct = 0;
            if (dSums != null && dSums.Any())
            {
                this.Impressions = dSums.Sum(ds => ds.Impressions);
                this.Clicks = dSums.Sum(ds => ds.Clicks);
                this.PostClickConv = dSums.Sum(ds => ds.PostClickConv);
                this.PostViewConv = dSums.Sum(ds => ds.PostViewConv);
                cost = dSums.Sum(ds => ds.Cost);
                if (roundCost)
                    cost = Math.Round(cost, 2);
            }
            if (marginFeeVals != null)
            {
                mgmtFeePct = marginFeeVals.MgmtFeePct;
                marginPct = marginFeeVals.MarginPct;
            }
            SetMoneyVals(cost, mgmtFeePct, marginPct);
        }

        // Computed properties
        public double CTR
        {
            get { return (Impressions == 0) ? 0 : Math.Round((double)Clicks / Impressions, 4); }
        }
        public double ConvRate
        {
            get { return (Clicks == 0) ? 0 : Math.Round((double)TotalConv / Clicks, 4); }
        }

        public decimal CPM
        {
            get { return (Impressions == 0) ? 0 : Math.Round(1000 * MediaSpend() / Impressions, 2); }
        }
        public decimal CPC
        {
            get { return (Clicks == 0) ? 0 : Math.Round(MediaSpend() / Clicks, 2); }
        }
        public decimal CPA
        {
            get { return (TotalConv == 0) ? 0 : Math.Round(MediaSpend() / TotalConv, 2); }
        }
    }

    //Allows for the computation of MediaSpend, MgmtFee, TotalRevenue, Margin...
    public class TDMoneyStat
    {
        public virtual void CopyFrom(TDMoneyStat stat)
        {
            this.Cost = stat.Cost;
            this.MgmtFeePct = stat.MgmtFeePct;
            this.MarginPct = stat.MarginPct;
        }
        public virtual bool AllZeros()
        {
            return (Cost == 0);
        }

        public decimal RawCost
        {
            set { Cost = value; }
        }
        public decimal Cost { get; set; } // this may or may not go through us
        public decimal MgmtFeePct { get; set; }
        public decimal MarginPct { get; set; }

        public TDMoneyStat() { }
        public TDMoneyStat(decimal mgmtFeePct, decimal marginPct = 0, decimal mediaSpend = 0)
        {
            this.MgmtFeePct = mgmtFeePct;
            this.MarginPct = marginPct;
            SetMediaSpend(mediaSpend);
        }

        public void SetMarginFees(MarginFeeVals marginFees)
        {
            if (marginFees != null)
            {
                this.MgmtFeePct = marginFees.MgmtFeePct;
                this.MarginPct = marginFees.MarginPct;
            }
            // else set to 0?
        }

        public void SetMoneyVals(decimal cost, decimal mgmtFeePct, decimal marginPct)
        {
            this.Cost = cost;
            this.MgmtFeePct = mgmtFeePct;
            this.MarginPct = marginPct;
        }

        // Compute and set Cost based on the specified MediaSpend
        public void SetMediaSpend(decimal mediaSpend)
        {
            if (MarginPct == 100)
                this.Cost = mediaSpend;
            else
                this.Cost = mediaSpend * MediaSpendToRevMultiplier() * RevToCostMultiplier();
        }

        //note: is 0 if MarginPct==100
        private decimal RevToCostMultiplier() // usually used in reverse (cost -> rev)
        {
            return (1 - MarginPct / 100);
        }
        private decimal MediaSpendToRevMultiplier() // usually used in reverse (rev -> mediaspend)
        {
            return (1 + MgmtFeePct / 100);
        }

        public decimal TotalRevenue()
        {
            if (MarginPct == 100)
                return Cost * MgmtFeePct / 100;
            else
                return Cost / RevToCostMultiplier();
        }

        public decimal MediaSpend()
        {
            if (MarginPct == 100)
                return Cost;
            else
                return Cost / RevToCostMultiplier() / MediaSpendToRevMultiplier();
                //return TotalRevenue() / MediaSpendToRevMultiplier();
        }

        public decimal MgmtFee()
        {
            if (MarginPct == 100)
                return TotalRevenue();
            else
                return TotalRevenue() - MediaSpend(); //TODO: make more efficient
        }

        public decimal DACost()
        {
            if (MarginPct == 100)
                return 0;
            else
                return Cost;
        }

        public decimal Margin()
        {
            return TotalRevenue() - DACost();
        }
    }
}
