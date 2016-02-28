using System;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.DTO
{
    public class TDStatsGauge
    {
        public ExtAccount ExtAccount { get; set; }

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
}
