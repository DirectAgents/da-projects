using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectAgents.Domain.Entities.CPProg
{
    public class DatedStatsSummaryWithRev : DatedStatsSummary
    {
        public decimal PostClickRev { get; set; }
        public decimal PostViewRev { get; set; }

        public override bool AllZeros()
        {
            return base.AllZeros() && PostClickRev == 0 && PostViewRev == 0;
        }

        public void SetStats(DatedStatsSummaryWithRev stat)
        {
            PostClickRev = stat.PostClickRev;
            PostViewRev = stat.PostViewRev;
            SetStats(stat as StatsSummary);
        }

        public void SetStats(DateTime date, IEnumerable<DatedStatsSummaryWithRev> stats)
        {
            SetStats(stats);
            Date = date;
            foreach (var metric in InitialMetrics)
            {
                metric.Date = date;
            }
        }

        public void SetStats(IEnumerable<DatedStatsSummaryWithRev> stats)
        {
            PostClickRev = stats.Sum(x => x.PostClickRev);
            PostViewRev = stats.Sum(x => x.PostViewRev);
            SetStats(stats as IEnumerable<StatsSummary>);
        }

        public void AddStats(DatedStatsSummaryWithRev stat)
        {
            PostClickRev += stat.PostClickRev;
            PostViewRev += stat.PostViewRev;
            AddStats(stat as StatsSummary);
        }
    }
}