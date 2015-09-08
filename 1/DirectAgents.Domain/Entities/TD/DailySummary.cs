using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
    }

    // DTOs:
    public class TDStat : TDMoneyStat
    {
        public string Name { get; set; }
        public ExtAccount ExtAccount { get; set; } // optional

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

        public bool AllZeros()
        {
            return (Impressions == 0 && Clicks == 0 && PostClickConv == 0 && PostViewConv == 0 && Cost == 0);
        }

        public void CopyFrom(TDStat tdStat)
        {
            this.Name = tdStat.Name;
            this.ExtAccount = tdStat.ExtAccount;

            this.Impressions = tdStat.Impressions;
            this.Clicks = tdStat.Clicks;
            this.PostClickConv = tdStat.PostClickConv;
            this.PostViewConv = tdStat.PostViewConv;
            this.Cost = tdStat.Cost;

            this.MgmtFeePct = tdStat.MgmtFeePct;
            this.MarginPct = tdStat.MarginPct;
        }

        public void SetStatsFrom(IEnumerable<DailySummary> dSums, bool roundCost = false)
        {
            if (dSums != null && dSums.Any())
            {
                this.Impressions = dSums.Sum(ds => ds.Impressions);
                this.Clicks = dSums.Sum(ds => ds.Clicks);
                this.PostClickConv = dSums.Sum(ds => ds.PostClickConv);
                this.PostViewConv = dSums.Sum(ds => ds.PostViewConv);
                this.Cost = dSums.Sum(ds => ds.Cost);
            }
            if (roundCost)
                this.Cost = Math.Round(this.Cost, 2);
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
            get { return (Impressions == 0) ? 0 : Math.Round(1000 * Cost / Impressions, 2); }
        }
        public decimal CPC
        {
            get { return (Clicks == 0) ? 0 : Math.Round(Cost / Clicks, 2); }
        }
        public decimal CPA
        {
            get { return (TotalConv == 0) ? 0 : Math.Round(Cost / TotalConv, 2); }
        }
    }

    //Allows for the computation of MediaSpend, MgmtFee, TotalRevenue, Margin...
    public class TDMoneyStat
    {
        public decimal Cost { get; set; }
        public decimal MgmtFeePct { get; set; }
        public decimal MarginPct { get; set; }

        public TDMoneyStat(decimal mgmtFeePct = 0, decimal marginPct = 0, decimal mediaSpend = 0)
        {
            this.MgmtFeePct = mgmtFeePct;
            this.MarginPct = marginPct;
            SetMediaSpend(mediaSpend);
        }

        public void SetMultipliers(BudgetInfo budgetInfo)
        {
            this.MgmtFeePct = budgetInfo.MgmtFeePct;
            this.MarginPct = budgetInfo.MarginPct;
        }

        // Compute and set Cost based on the specified MediaSpend
        public void SetMediaSpend(decimal mediaSpend)
        {
            this.Cost = mediaSpend * MediaSpendToRevMultiplier() * RevToCostMultiplier();
        }

        public decimal RevToCostMultiplier() // usually used in reverse (cost -> rev)
        {
            return (1 - MarginPct / 100);
        }
        public decimal MediaSpendToRevMultiplier() // usually used in reverse (rev -> mediaspend)
        {
            return (1 + MgmtFeePct / 100);
        }

        public decimal TotalRevenue()
        {
            return this.Cost / RevToCostMultiplier();
        }

        public decimal MediaSpend()
        {
            return TotalRevenue() / MediaSpendToRevMultiplier();
        }

        public decimal MgmtFee()
        {
            return TotalRevenue() - MediaSpend();
        }

        public decimal Margin()
        {
            return TotalRevenue() - this.Cost;
        }
    }
}
