using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg
{
    public class StatsSummary
    {
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int PostClickConv { get; set; }
        public int PostViewConv { get; set; }
        public decimal Cost { get; set; }

        //TotalConv

        public virtual bool AllZeros()
        {
            return (Impressions == 0 && Clicks == 0 && PostClickConv == 0 && PostViewConv == 0 && Cost == 0);
        }
    }

    public class DatedStatsSummary : StatsSummary
    {
        public DateTime Date { get; set; }
    }
    public class DatedStatsSummaryWithRev : DatedStatsSummary
    {
        public decimal PostClickRev { get; set; }
        public decimal PostViewRev { get; set; }

        public override bool AllZeros()
        {
            return base.AllZeros() && PostClickRev == 0 && PostViewRev == 0;
        }
    }

    public class DailySummary : DatedStatsSummaryWithRev
    {
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }
    }

    // DailySummary for a Strategy
    public class StrategySummary : DatedStatsSummaryWithRev
    {
        public int StrategyId { get; set; }
        public virtual Strategy Strategy { get; set; }

        [NotMapped]
        public string StrategyName { get; set; }
        [NotMapped]
        public string StrategyEid { get; set; } // external id
    }

    // DailySummary for an AdSet
    public class AdSetSummary : DatedStatsSummaryWithRev
    {
        public int AdSetId { get; set; }
        public virtual AdSet AdSet { get; set; }

        [NotMapped]
        public string AdSetName { get; set; }
        [NotMapped]
        public string AdSetEid { get; set; } // external id
        [NotMapped]
        public string StrategyName { get; set; }
        [NotMapped]
        public string StrategyEid { get; set; } // external id
    }

    // DailySummary for a "TD ad"
    public class TDadSummary : DatedStatsSummary
    {
        public int TDadId { get; set; }
        public virtual TDad TDad { get; set; }

        [NotMapped]
        public string TDadName { get; set; }
        [NotMapped]
        public string TDadEid { get; set; } // external id
        [NotMapped]
        public int Width { get; set; }
        //[NotMapped]
    }

    // DailySummary for a Site / ExtAccount
    public class SiteSummary : DatedStatsSummary
    {
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
