using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace DirectAgents.Web.Helpers
{
    public static class UIMetricHelper
    {
        public static IEnumerable<SummaryMetric> GetSumMetrics(IEnumerable<TDRawStat> stats)
        {
            var metrics = GetStatsAllMetrics(stats);
            return MetricHelper.GetSumMetrics(metrics);
        }

        public static Dictionary<string, int> GetMetricTypeDictionary(IEnumerable<TDRawStat> stats)
        {
            var metrics = GetStatsAllMetrics(stats);
            return GetMetricTypeDictionary(metrics);
        }

        public static Dictionary<string, int> GetMetricTypeDictionary(IEnumerable<DailySummary> stats)
        {
            var metrics = GetSummariesAllMetrics(stats);
            return GetMetricTypeDictionary(metrics);
        }

        private static IEnumerable<SummaryMetric> GetStatsAllMetrics(IEnumerable<TDRawStat> stats)
        {
            var metrics = stats.Where(x => x?.Metrics != null).SelectMany(x => x.Metrics);
            return metrics.ToList();
        }

        private static IEnumerable<SummaryMetric> GetSummariesAllMetrics(IEnumerable<DailySummary> stats)
        {
            var metrics = stats.Where(x => x?.Metrics != null).SelectMany(x => x.Metrics);
            return metrics.ToList();
        }

        private static Dictionary<string, int> GetMetricTypeDictionary(IEnumerable<SummaryMetric> metrics)
        {
            var groupedMetrics = metrics.GroupBy(x => GetMetricHeader(x.MetricType));
            return groupedMetrics.ToDictionary(x => x.Key, x => x.First().MetricType.Id);
        }

        private static string GetMetricHeader(MetricType metric)
        {
            var daysHeader = metric.DaysInterval != 0
                ? $" ({metric.DaysInterval} day(s)" : "";
            return metric.Name + (metric.DaysInterval != 0 ? $" - {metric.DaysInterval} day(s)" : "");
        }
    }
}