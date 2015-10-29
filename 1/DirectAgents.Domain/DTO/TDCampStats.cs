using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.DTO
{
    public class TDCampStats : TDBasicStat
    {
        public Campaign Campaign { get; set; }
        public IEnumerable<TDStatWithBudget> PlatformStats { get; set; }
        public DateTime Month;
        public TDBudget Budget;

        public TDCampStats(Campaign campaign, IEnumerable<TDStatWithBudget> platformStats, DateTime monthStart, decimal? budget = null)
            : base(platformStats)
        {
            Campaign = campaign;
            PlatformStats = platformStats;
            Month = monthStart;
            if (budget.HasValue)
                Budget.MediaSpend = budget.Value;
        }

        public decimal FractionReached()
        {
            if (Budget.MediaSpend == 0)
                return 0;
            else
                return this.MediaSpend / Budget.MediaSpend;
        }
    }

    public struct TDBudget
    {
        public decimal MediaSpend;
    }

    public class TDBasicStat
    {
        public TDBasicStat(IEnumerable<TDMediaStat> statsToSum)
        {
            Impressions = statsToSum.Sum(s => s.Impressions);
            Clicks = statsToSum.Sum(s => s.Clicks);
            PostClickConv = statsToSum.Sum(s => s.PostClickConv);
            PostViewConv = statsToSum.Sum(s => s.PostViewConv);
            DACost = statsToSum.Sum(s => s.DACost());
            MediaSpend = statsToSum.Sum(s => s.MediaSpend());
            MgmtFee = statsToSum.Sum(s => s.MgmtFee());
            TotalRevenue = statsToSum.Sum(s => s.TotalRevenue());
        }

        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int PostClickConv { get; set; }
        public int PostViewConv { get; set; }
        public decimal DACost { get; set; }
        public decimal MediaSpend { get; set; } //TODO: chg to ClientCost
        public decimal MgmtFee { get; set; }
        public decimal TotalRevenue { get; set; }

        public bool AllZeros()
        {
            return (Impressions == 0 && Clicks == 0 && PostClickConv == 0 && PostViewConv == 0 && DACost == 0 && MediaSpend == 0 && TotalRevenue == 0);
        }

        // Computed properties
        public int TotalConv
        {
            get { return PostClickConv + PostViewConv; }
        }
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
            get { return (Impressions == 0) ? 0 : Math.Round(1000 * MediaSpend / Impressions, 2); }
        }
        public decimal CPC
        {
            get { return (Clicks == 0) ? 0 : Math.Round(MediaSpend / Clicks, 2); }
        }
        public decimal CPA
        {
            get { return (TotalConv == 0) ? 0 : Math.Round(MediaSpend / TotalConv, 2); }
        }

        public decimal Margin
        {
            get { return TotalRevenue - DACost; }
        }
        public decimal MarginPct //TODO: make nullable
        {
            get { return (TotalRevenue == 0) ? 0 : (100 * Margin / TotalRevenue); }
        }

        //public decimal MgmtFeePct
    }

}
