using DirectAgents.Domain.Entities.CPProg;
using System.Collections.Generic;
using System.Linq;

namespace DirectAgents.Domain.Helpers
{
    public static class MetricHelper
    {
        public static IEnumerable<SummaryMetric> GetSumMetrics(IEnumerable<StatsSummary> sSums)
        {
            var metrics = sSums.Where(x => x?.InitialMetrics != null).SelectMany(x => x.InitialMetrics);
            return GetSumMetrics(metrics);
        }

        public static IEnumerable<SummaryMetric> GetSumMetrics(IEnumerable<SummaryMetric> metrics)
        {
            var groupedMetrics = metrics.GroupBy(x => x.MetricType);
            var sumMetrics = groupedMetrics.Select(x => new SummaryMetric
            {
                MetricType = x.Key,
                Value = x.Sum(m => m.Value)
            });
            return sumMetrics.ToList();
        }
    }
}
