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
        public TDBudget Budget;
        // Date?

        public TDCampStats(Campaign campaign, IEnumerable<TDStatWithBudget> platformStats, decimal? budget = null)
            : base(platformStats)
        {
            Campaign = campaign;
            PlatformStats = platformStats;
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
        public TDBasicStat(IEnumerable<TDStat> statsToSum)
        {
            Impressions = statsToSum.Sum(s => s.Impressions);
            Clicks = statsToSum.Sum(s => s.Clicks);
            PostClickConv = statsToSum.Sum(s => s.PostClickConv);
            PostViewConv = statsToSum.Sum(s => s.PostViewConv);
            Cost = statsToSum.Sum(s => s.Cost);
            MediaSpend = statsToSum.Sum(s => s.MediaSpend());
            TotalRevenue = statsToSum.Sum(s => s.TotalRevenue());
        }

        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int PostClickConv { get; set; }
        public int PostViewConv { get; set; }
        public decimal Cost { get; set; }
        public decimal MediaSpend { get; set; }
        public decimal TotalRevenue { get; set; }

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
            get { return TotalRevenue - Cost; }
        }
        public decimal MarginPct
        {
            get { return (TotalRevenue == 0) ? 0 : (100 * Margin / TotalRevenue); }
        }

        // MgmtFee
    }

}
