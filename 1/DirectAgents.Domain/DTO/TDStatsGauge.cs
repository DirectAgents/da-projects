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

    public class StatsSummaryRange : IStatsRange
    {
        public IQueryable<DatedStatsSummary> Summaries { get; set; }

        public StatsSummaryRange(IQueryable<DatedStatsSummary> summaries)
        {
            this.Summaries = summaries;
        }

        public DateTime? Earliest
        {
            get { return (Summaries == null || !Summaries.Any()) ? null : (DateTime?)Summaries.Min(s => s.Date); }
        }
        public DateTime? Latest
        {
            get { return (Summaries == null || !Summaries.Any()) ? null : (DateTime?)Summaries.Max(s => s.Date); }
        }
        //TODO? cache the results
    }
    public class ConvRange : IStatsRange
    {
        public IQueryable<Conv> Convs { get; set; }

        public ConvRange(IQueryable<Conv> convs)
        {
            this.Convs = convs;
        }

        public DateTime? Earliest
        {
            get { return (Convs == null || !Convs.Any()) ? null : (DateTime?)Convs.Min(s => s.Time); }
        }
        public DateTime? Latest
        {
            get { return (Convs == null || !Convs.Any()) ? null : (DateTime?)Convs.Max(s => s.Time); }
        }
        //TODO? cache the results
    }
}
