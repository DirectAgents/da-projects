using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.DTO
{
    public class TDMediaStatWithBudget : TDMediaStat
    {
        //public DateTime Date { get; set; } // the month of the budget
        public TDBudget Budget;

        //public IEnumerable<TDStat> ExtAccountStats { get; set; }

        public TDMediaStatWithBudget(IEnumerable<DailySummary> dSums, BudgetInfoVals budgetVals)
            : base(dSums, budgetVals)
        {
            Budget.MediaSpend = budgetVals.MediaSpend;
        }

        //public TDStatWithBudget(TDStat tdStat, BudgetInfo budgetInfo)
        //{
        //    CopyFrom(tdStat);
        //    Budget.MediaSpend = budgetInfo.MediaSpend;
        //}

        public decimal FractionReached()
        {
            if (Budget.MediaSpend == 0)
                return 0;
            return this.MediaSpend() / Budget.MediaSpend;
        }

    }

    //Allows for the computation of MediaSpend, MgmtFee, TotalRevenue, Margin...
    public class TDMediaStat : TDRawStat
    {
        public override void CopyFrom(TDRawStat stat)
        {
            base.CopyFrom(stat);
            if (stat is TDMediaStat)
            {
                this.MgmtFeePct = ((TDMediaStat)stat).MgmtFeePct;
                this.MarginPct = ((TDMediaStat)stat).MarginPct;
            }
        }
        public decimal MgmtFeePct { get; set; }
        public decimal MarginPct { get; set; }

        public TDMediaStat() { }
        //public TDMediaStat(decimal mgmtFeePct, decimal marginPct = 0, decimal mediaSpend = 0)
        //{
        //    this.MgmtFeePct = mgmtFeePct;
        //    this.MarginPct = marginPct;
        //    SetMediaSpend(mediaSpend);
        //}
        public TDMediaStat(IEnumerable<DailySummary> dSums, MarginFeeVals marginFees)
            : base(dSums)
        {
            SetMarginFees(marginFees);
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

        public override decimal CPM
        {
            get { return (Impressions == 0) ? 0 : Math.Round(1000 * MediaSpend() / Impressions, 2); }
        }
        public override decimal CPC
        {
            get { return (Clicks == 0) ? 0 : Math.Round(MediaSpend() / Clicks, 2); }
        }
        public override decimal CPA
        {
            get { return (TotalConv == 0) ? 0 : Math.Round(MediaSpend() / TotalConv, 2); }
        }
    }

    public class TDRawStat
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
        public decimal Cost { get; set; }
        //public int Prospects { get; set; }

        public int TotalConv
        {
            get { return PostClickConv + PostViewConv; }
        }

        public virtual bool AllZeros()
        {
            return (Impressions == 0 && Clicks == 0 && PostClickConv == 0 && PostViewConv == 0 && Cost == 0);
        }

        public virtual void CopyFrom(TDRawStat stat)
        {
            this.Name = stat.Name;
            this.Campaign = stat.Campaign;
            this.ExtAccount = stat.ExtAccount;
            this.Platform = stat.Platform;

            this.Impressions = stat.Impressions;
            this.Clicks = stat.Clicks;
            this.PostClickConv = stat.PostClickConv;
            this.PostViewConv = stat.PostViewConv;
        }

        // Constructors
        public TDRawStat() { }
        public TDRawStat(IEnumerable<DailySummary> dSums)
        {
            SetStatsFrom(dSums);
        }

        private void SetStatsFrom(IEnumerable<DailySummary> dSums, bool roundCost = false)
        {
            if (dSums != null && dSums.Any())
            {
                this.Impressions = dSums.Sum(ds => ds.Impressions);
                this.Clicks = dSums.Sum(ds => ds.Clicks);
                this.PostClickConv = dSums.Sum(ds => ds.PostClickConv);
                this.PostViewConv = dSums.Sum(ds => ds.PostViewConv);
                this.Cost = dSums.Sum(ds => ds.Cost);
                if (roundCost)
                    this.Cost = Math.Round(this.Cost, 2);
            }
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

        public virtual decimal CPM
        {
            get { return (Impressions == 0) ? 0 : Math.Round(1000 * Cost / Impressions, 2); }
        }
        public virtual decimal CPC
        {
            get { return (Clicks == 0) ? 0 : Math.Round(Cost / Clicks, 2); }
        }
        public virtual decimal CPA
        {
            get { return (TotalConv == 0) ? 0 : Math.Round(Cost / TotalConv, 2); }
        }
    }
}
