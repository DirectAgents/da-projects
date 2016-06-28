using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.DTO
{
    public class TDStatsGauge
    {
        public Platform Platform { get; set; }
        public ExtAccount ExtAccount { get; set; }

        public IEnumerable<TDStatsGauge> Children { get; set; }

        public TDStatRange Daily;
        public TDStatRange Strategy;
        public TDStatRange Creative;
        public TDStatRange Site;
        public TDStatRange Conv;

        public void Reset()
        {
            Daily.Earliest = null;
            Daily.Latest = null;
            Strategy.Earliest = null;
            Strategy.Latest = null;
            Creative.Earliest = null;
            Creative.Latest = null;
            Site.Earliest = null;
            Site.Latest = null;
            Conv.Earliest = null;
            Conv.Latest = null;
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
    }

    public interface IStatsRange
    {
        DateTime? Earliest { get; }
        DateTime? Latest { get; }
    }

    public class SimpleStatsRange : IStatsRange
    {
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
        public IQueryable<DatedStatsSummary> Summaries { get; set; }

        public StatsSummaryRange(IQueryable<DatedStatsSummary> summaries)
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

        private bool earliestSet;
        private DateTime? _earliest;
        public DateTime? Earliest
        {
            get
            {
                if (!earliestSet)
                {
                    _earliest = (Convs == null || !Convs.Any()) ? null : (DateTime?)Convs.Min(s => s.Time);
                    earliestSet = true;
                }
                return _earliest;
            }
        }

        private bool latestSet;
        private DateTime? _latest;
        public DateTime? Latest
        {
            get
            {
                if (!latestSet)
                {
                    _latest = (Convs == null || !Convs.Any()) ? null : (DateTime?)Convs.Max(s => s.Time);
                    latestSet = true;
                }
                return _latest;
            }
        }
    }
}
