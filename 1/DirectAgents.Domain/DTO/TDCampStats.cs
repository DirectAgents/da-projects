using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.DTO
{
    public class TDCampStats : TDLineItem, ITDLineItem
    {
        public Campaign Campaign { get; set; }
        public Platform Platform { get; set; }
        public IEnumerable<ITDLineItem> LineItems { get; set; }
        public DateTime Month;
        public ITDBudget Budget { get; private set; }

        public TDCampStats(Campaign campaign, IEnumerable<ITDLineItem> platformStats, DateTime monthStart, decimal? budget = null)
            : base(platformStats)
        {
            Campaign = campaign;
            LineItems = platformStats;
            Month = monthStart;
            Budget = new TDBudget(budget);
        }

        public double FractionReached()
        {
            if (Budget.ClientCost == 0)
                return 0;
            else
                return (double)(this.ClientCost / Budget.ClientCost);
        }
    }

    public class TDBudget : ITDBudget
    {
        public decimal ClientCost { get; set; } //TODO: make nullable

        public TDBudget(decimal? clientCost)
        {
            ClientCost = clientCost.HasValue ? clientCost.Value : 0;
        }
    }

    public class TDLineItem : TDRawLineItem//, ITDLineItem
    {
        public override bool AllZeros(bool includeClientCost = true)
        {
            bool allZeros = base.AllZeros(includeClientCost);
            return (allZeros && Impressions == 0 && Clicks == 0 && PostClickConv == 0 && PostViewConv == 0);
        }

        // Constructor
        public TDLineItem(IEnumerable<ITDLineItem> statsToSum)
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
