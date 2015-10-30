using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.DTO
{
    public class TDCampStats : TDLineItem
    {
        public Campaign Campaign { get; set; }
        public IEnumerable<TDMediaStatWithBudget> PlatformStats { get; set; }
        public DateTime Month;
        public TDBudget Budget;

        public TDCampStats(Campaign campaign, IEnumerable<TDMediaStatWithBudget> platformStats, DateTime monthStart, decimal? budget = null)
            : base(platformStats)
        {
            Campaign = campaign;
            PlatformStats = platformStats;
            Month = monthStart;
            if (budget.HasValue)
                Budget.ClientCost = budget.Value;
        }

        public decimal FractionReached()
        {
            if (Budget.ClientCost == 0)
                return 0;
            else
                return this.ClientCost / Budget.ClientCost;
        }
    }

    public struct TDBudget
    {
        public decimal ClientCost;
    }

    public class TDLineItem : TDRawLineItem, ITDLineItem
    {
        public override bool AllZeros(bool includeClientCost = true)
        {
            bool allZeros = base.AllZeros(includeClientCost);
            return (allZeros && Impressions == 0 && Clicks == 0 && PostClickConv == 0 && PostViewConv == 0);
        }

        // Constructor
        public TDLineItem(IEnumerable<TDMediaStat> statsToSum)
            : base(statsToSum)
        {
            Impressions = statsToSum.Sum(s => s.Impressions);
            Clicks = statsToSum.Sum(s => s.Clicks);
            PostClickConv = statsToSum.Sum(s => s.PostClickConv);
            PostViewConv = statsToSum.Sum(s => s.PostViewConv);
        }

        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int PostClickConv { get; set; }
        public int PostViewConv { get; set; }

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
            get { return (Impressions == 0) ? 0 : Math.Round(1000 * ClientCost / Impressions, 2); }
        }
        public decimal CPC
        {
            get { return (Clicks == 0) ? 0 : Math.Round(ClientCost / Clicks, 2); }
        }
        public decimal CPA
        {
            get { return (TotalConv == 0) ? 0 : Math.Round(ClientCost / TotalConv, 2); }
        }

    }

}
