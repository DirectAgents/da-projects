using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.TD
{
    public class DaySumStats
    {
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int PostClickConv { get; set; }
        public int PostViewConv { get; set; }
        public decimal Cost { get; set; }

        //TotalConv

        public bool AllZeros()
        {
            return (Impressions == 0 && Clicks == 0 && PostClickConv == 0 && PostViewConv == 0 && Cost == 0);
        }
    }

    public class DailySummary : DaySumStats
    {
        public DateTime Date { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }
    }

    public class StrategySummary : DaySumStats
    {
        public DateTime Date { get; set; }
        public int StrategyId { get; set; }
        public virtual Strategy Strategy { get; set; }
    }

    public class TDadSummary : DaySumStats
    {
        public DateTime Date { get; set; }
        public int TDadId { get; set; }
        public virtual TDad TDad { get; set; }
    }

    // DailySummary for a Site / ExtAccount
    public class SiteSummary : DaySumStats
    {
        public DateTime Date { get; set; }

        public int SiteId { get; set; }
        public virtual Site Site { get; set; }

        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }
    }
    public class Site
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
