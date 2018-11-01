using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DirectAgents.Domain.Entities.CPProg
{
    public class StatsSummary
    {
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int AllClicks { get; set; }
        public int PostClickConv { get; set; }
        public int PostViewConv { get; set; }
        public decimal Cost { get; set; }
        public virtual IEnumerable<SummaryMetric> Metrics { get; set; }

        //TotalConv

        public virtual bool AllZeros()
        {
            return Impressions == 0 && Clicks == 0 && PostClickConv == 0 && PostViewConv == 0 && Cost == 0 && (Metrics == null || !Metrics.Any());
        }

        public virtual void SetStats(StatsSummary stat)
        {
            Impressions = stat.Impressions;
            Clicks = stat.Clicks;
            AllClicks = stat.AllClicks;
            PostClickConv = stat.PostClickConv;
            PostViewConv = stat.PostViewConv;
            Cost = stat.Cost;
        }
        public void SetStats(IEnumerable<StatsSummary> stats)
        {
            SetBasicStats(stats);
        } // (to avoid naming conflict in derived class)
        protected void SetBasicStats(IEnumerable<StatsSummary> stats)
        {
            Impressions = stats.Sum(x => x.Impressions);
            Clicks = stats.Sum(x => x.Clicks);
            AllClicks = stats.Sum(x => x.AllClicks);
            PostClickConv = stats.Sum(x => x.PostClickConv);
            PostViewConv = stats.Sum(x => x.PostViewConv);
            Cost = stats.Sum(x => x.Cost);
        }
    }

    public interface IDatedObject
    {
        DateTime Date { get; }
    }

    public class DatedStatsSummary : StatsSummary, IDatedObject
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
        public override void SetStats(StatsSummary stat)
        {
            base.SetStats(stat);
            if (stat is DatedStatsSummaryWithRev)
            {
                PostClickRev = ((DatedStatsSummaryWithRev)stat).PostClickRev;
                PostViewRev = ((DatedStatsSummaryWithRev)stat).PostViewRev;
            }
        }
        public void SetStats(IEnumerable<DatedStatsSummaryWithRev> stats)
        {
            SetBasicStats(stats);
            PostClickRev = stats.Sum(x => x.PostClickRev);
            PostViewRev = stats.Sum(x => x.PostViewRev);
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
        [NotMapped]
        public string StrategyType { get; set; }
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
        public string AdSetName { get; set; }
        [NotMapped]
        public string AdSetEid { get; set; }
        [NotMapped]
        public IEnumerable<TDadExternalId> ExternalIds { get; set; }
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

    // DailySummary for a Keyword
    public class KeywordSummary : DatedStatsSummary
    {
        public int KeywordId { get; set; }
        public virtual Keyword Keyword { get; set; }

        [NotMapped]
        public string KeywordName { get; set; }
        [NotMapped]
        public string KeywordEid { get; set; }
        [NotMapped]
        public string AdSetName { get; set; }
        [NotMapped]
        public string AdSetEid { get; set; }
        [NotMapped]
        public string StrategyName { get; set; }
        [NotMapped]
        public string StrategyEid { get; set; }
    }

    // DailySummary for a SearchTerm
    public class SearchTermSummary : DatedStatsSummary
    {
        public int SearchTermId { get; set; }
        public virtual SearchTerm SearchTerm { get; set; }
    }

    public class Site
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MetricType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? DaysInterval { get; set; }
    }

    public class SummaryMetric
    {
        public int EntityId { get; set; }
        public DateTime Date { get; set; }

        public int MetricTypeId { get; set; }
        public virtual MetricType MetricType { get; set; }

        public decimal Value { get; set; }
    }

    public class DailySummaryMetric : SummaryMetric
    {
        [ForeignKey("EntityId")]
        public virtual ExtAccount ExtAccount { get; set; }
    }

    public class TDadSummaryMetric : SummaryMetric
    {
        [ForeignKey("EntityId")]
        public virtual TDad TDad { get; set; }
    }

    public class AdSetSummaryMetric : SummaryMetric
    {
        [ForeignKey("EntityId")]
        public virtual AdSet AdSet { get; set; }
    }

    public class StrategySummaryMetric : SummaryMetric
    {
        [ForeignKey("EntityId")]
        public virtual Strategy Strategy { get; set; }
    }

    public class KeywordSummaryMetric : SummaryMetric
    {
        [ForeignKey("EntityId")]
        public virtual Keyword Keyword { get; set; }
    }

    public class SearchTermSummaryMetric : SummaryMetric
    {
        [ForeignKey("EntityId")]
        public virtual SearchTerm SearchTerm { get; set; }
    }
}
