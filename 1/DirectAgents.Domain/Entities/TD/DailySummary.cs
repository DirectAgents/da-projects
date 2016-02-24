using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.TD
{
    public class StatsSummary
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

    public class DailySummary : StatsSummary
    {
        public DateTime Date { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }
    }

    // DailySummary for a Strategy
    public class StrategySummary : StatsSummary
    {
        public DateTime Date { get; set; }
        public int StrategyId { get; set; }
        public virtual Strategy Strategy { get; set; }

        [NotMapped]
        public string StrategyName { get; set; }
        [NotMapped]
        public string StrategyEid { get; set; } // external id
    }

    // DailySummary for a "TD ad"
    public class TDadSummary : StatsSummary
    {
        public DateTime Date { get; set; }
        public int TDadId { get; set; }
        public virtual TDad TDad { get; set; }

        [NotMapped]
        public string TDadName { get; set; }
        [NotMapped]
        public string TDadEid { get; set; } // external id
    }

    // DailySummary for a Site / ExtAccount
    public class SiteSummary : StatsSummary
    {
        public DateTime Date { get; set; }

        public int SiteId { get; set; }
        public virtual Site Site { get; set; }

        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }

        [NotMapped]
        public string SiteName
        {
            get { return _sitename; }
            set { _sitename = (value == null) ? null : value.ToLower(); }
        }
        private string _sitename;
    }

    public class Site
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
