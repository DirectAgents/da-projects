using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Domain.DTO
{
    public class TDStatsGauge
    {
        public TDStatsGauge()
        { }
        public TDStatsGauge(IStatsRange dailyRange, IStatsRange stratRange, IStatsRange adsetRange, IStatsRange actionRange, IStatsRange creativeRange, IStatsRange keywRange, IStatsRange stermRange, IStatsRange siteRange, IStatsRange convRange)
        {
            if (dailyRange != null)
            {
                if (dailyRange.Earliest.HasValue)
                    this.Daily.Earliest = dailyRange.Earliest.Value;
                if (dailyRange.Latest.HasValue)
                    this.Daily.Latest = dailyRange.Latest.Value;
            }
            if (stratRange != null)
            {
                if (stratRange.Earliest.HasValue)
                    this.Strategy.Earliest = stratRange.Earliest.Value;
                if (stratRange.Latest.HasValue)
                    this.Strategy.Latest = stratRange.Latest.Value;
            }
            if (adsetRange != null)
            {
                if (adsetRange.Earliest.HasValue)
                    this.AdSet.Earliest = adsetRange.Earliest.Value;
                if (adsetRange.Latest.HasValue)
                    this.AdSet.Latest = adsetRange.Latest.Value;
            }
            if (actionRange != null)
            {
                if (actionRange.Earliest.HasValue)
                    this.Action.Earliest = actionRange.Earliest.Value;
                if (actionRange.Latest.HasValue)
                    this.Action.Latest = actionRange.Latest.Value;
            }
            if (creativeRange != null)
            {
                if (creativeRange.Earliest.HasValue)
                    this.Creative.Earliest = creativeRange.Earliest.Value;
                if (creativeRange.Latest.HasValue)
                    this.Creative.Latest = creativeRange.Latest.Value;
            }
            if (keywRange != null)
            {
                if (keywRange.Earliest.HasValue)
                    this.Keyword.Earliest = keywRange.Earliest.Value;
                if (keywRange.Latest.HasValue)
                    this.Keyword.Latest = keywRange.Latest.Value;
            }
            if (stermRange != null)
            {
                if (stermRange.Earliest.HasValue)
                    this.SearchTerm.Earliest = stermRange.Earliest.Value;
                if (stermRange.Latest.HasValue)
                    this.SearchTerm.Latest = stermRange.Latest.Value;
            }
            if (siteRange != null)
            {
                if (siteRange.Earliest.HasValue)
                    this.Site.Earliest = siteRange.Earliest.Value;
                if (siteRange.Latest.HasValue)
                    this.Site.Latest = siteRange.Latest.Value;
            }
            if (convRange != null)
            {
                if (convRange.Earliest.HasValue)
                    this.Conv.Earliest = convRange.Earliest.Value;
                if (convRange.Latest.HasValue)
                    this.Conv.Latest = convRange.Latest.Value;
            }
        }

        public Platform Platform { get; set; }
        public ExtAccount ExtAccount { get; set; }

        public IEnumerable<TDStatsGauge> Children { get; set; }

        //TODO: replace with IStatsRanges?

        public TDStatRange Daily;
        public TDStatRange Strategy;
        public TDStatRange Creative;
        public TDStatRange AdSet;
        public TDStatRange Keyword;
        public TDStatRange SearchTerm;
        public TDStatRange Site;
        public TDStatRange Conv;
        public TDStatRange Action;

        public bool HasStats()
        {
            return Daily.HasStats() || Strategy.HasStats() || AdSet.HasStats() || Action.HasStats() || Creative.HasStats() || Keyword.HasStats() || SearchTerm.HasStats() || Site.HasStats() || Conv.HasStats();
        }

        //public void Reset()
        //{
        //    Daily.Earliest = null;
        //    Daily.Latest = null;
        //    Strategy.Earliest = null;
        //    Strategy.Latest = null;
        //    Creative.Earliest = null;
        //    Creative.Latest = null;
        //    AdSet.Earliest = null;
        //    AdSet.Latest = null;
        //    Site.Earliest = null;
        //    Site.Latest = null;
        //    Conv.Earliest = null;
        //    Conv.Latest = null;
        //    Action.Earliest = null;
        //    Action.Latest = null;
        //}
        public void SetFrom(IEnumerable<TDStatsGauge> gauges)
        {
            this.Daily.Earliest = gauges.Min(x => x.Daily.Earliest);
            this.Daily.Latest = gauges.Max(x => x.Daily.Latest);
            this.Strategy.Earliest = gauges.Min(x => x.Strategy.Earliest);
            this.Strategy.Latest = gauges.Max(x => x.Strategy.Latest);
            this.Creative.Earliest = gauges.Min(x => x.Creative.Earliest);
            this.Creative.Latest = gauges.Max(x => x.Creative.Latest);
            this.AdSet.Earliest = gauges.Min(x => x.AdSet.Earliest);
            this.AdSet.Latest = gauges.Max(x => x.AdSet.Latest);
            this.Keyword.Earliest = gauges.Min(x => x.Keyword.Earliest);
            this.Keyword.Latest = gauges.Max(x => x.Keyword.Latest);
            this.SearchTerm.Earliest = gauges.Min(x => x.SearchTerm.Earliest);
            this.SearchTerm.Latest = gauges.Max(x => x.SearchTerm.Latest);
            this.Site.Earliest = gauges.Min(x => x.Site.Earliest);
            this.Site.Latest = gauges.Max(x => x.Site.Latest);
            this.Conv.Earliest = gauges.Min(x => x.Conv.Earliest);
            this.Conv.Latest = gauges.Max(x => x.Conv.Latest);
            this.Action.Earliest = gauges.Min(x => x.Action.Earliest);
            this.Action.Latest = gauges.Max(x => x.Action.Latest);
        }
    }
    public struct TDStatRange
    {
        public DateTime? Earliest { get; set; }
        public DateTime? Latest { get; set; }

        public string EarliestDisp
        {
            get { return Earliest.HasValue ? Earliest.Value.ToShortDateString() : null; }
        }
        public string LatestDisp
        {
            get { return Latest.HasValue ? Latest.Value.ToShortDateString() : null; }
        }

        public bool HasStats()
        {
            return Earliest.HasValue || Latest.HasValue;
        }
    }

    public interface IStatsRange
    {
        DateTime? Earliest { get; }
        DateTime? Latest { get; }
    }

    public class SimpleStatsRange : IStatsRange
    {
        public static IEnumerable<SimpleStatsRange> EmptyIEnumerable()
        { return new List<SimpleStatsRange>(); }

        public SimpleStatsRange()
        { }
        public SimpleStatsRange(DateTime? earliest, DateTime? latest)
        {
            this.Earliest = earliest;
            this.Latest = latest;
        }

        public int Id { get; set; }

        public DateTime? Earliest { get; set; }
        public DateTime? Latest { get; set; }

        public void UpdateWith(IStatsRange statsRange)
        {
            SetIfEarlier(statsRange.Earliest);
            SetIfLater(statsRange.Latest);
        }
        public void SetIfEarlier(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return;
            if (!this.Earliest.HasValue || dateTime.Value < this.Earliest.Value)
                this.Earliest = dateTime;
        }
        public void SetIfLater(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return;
            if (!this.Latest.HasValue || dateTime.Value > this.Latest.Value)
                this.Latest = dateTime;
        }
    }
    public class StatsSummaryRange : IStatsRange
    {
        public IQueryable<IDatedObject> Summaries { get; set; }

        public StatsSummaryRange(IQueryable<IDatedObject> summaries)
        {
            this.Summaries = summaries;
        }

        private bool? _anySums;
        private bool AnySums
        {
            get
            {
                if (!_anySums.HasValue)
                {
                    _anySums = (Summaries != null && Summaries.Any());
                }
                return _anySums.Value;
            }
        }

        private DateTime? _earliest;
        public DateTime? Earliest
        {
            get
            {
                if (!_earliest.HasValue && this.AnySums)
                {
                    _earliest = Summaries.Min(s => s.Date);
                }
                return _earliest;
            }
        }
        private DateTime? _latest;
        public DateTime? Latest
        {
            get
            {
                if (!_latest.HasValue && this.AnySums)
                {
                    _latest = Summaries.Max(s => s.Date);
                }
                return _latest;
            }
        }
    }

    public class ConvRange : IStatsRange
    {
        public IQueryable<Conv> Convs { get; set; }

        public ConvRange(IQueryable<Conv> convs)
        {
            this.Convs = convs;
        }

        private bool? _anyConvs;
        private bool AnyConvs
        {
            get
            {
                if (!_anyConvs.HasValue)
                {
                    _anyConvs = (Convs != null && Convs.Any());
                }
                return _anyConvs.Value;
            }
        }

        private DateTime? _earliest;
        public DateTime? Earliest
        {
            get
            {
                if (!_earliest.HasValue && this.AnyConvs)
                {
                    _earliest = Convs.Min(x => x.Time);
                }
                return _earliest;
            }
        }

        private DateTime? _latest;
        public DateTime? Latest
        {
            get
            {
                if (!_latest.HasValue && this.AnyConvs)
                {
                    _earliest = Convs.Max(x => x.Time);
                }
                return _latest;
            }
        }
    }
}
