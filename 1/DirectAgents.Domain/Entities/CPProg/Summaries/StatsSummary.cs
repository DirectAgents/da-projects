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
        [NotMapped]
        public IEnumerable<SummaryMetric> InitialMetrics { get; set; }

        //TotalConv

        public virtual bool AllZeros()
        {
            return Impressions == 0 && Clicks == 0 && PostClickConv == 0 && PostViewConv == 0 && Cost == 0 && (InitialMetrics == null || !InitialMetrics.Any());
        }

        public void AddStats(StatsSummary stat)
        {
            AddBasicStats(stat);
            AddMetricStats(stat);
        }

        public void SetStats(StatsSummary stat)
        {
            Impressions = stat.Impressions;
            Clicks = stat.Clicks;
            AllClicks = stat.AllClicks;
            PostClickConv = stat.PostClickConv;
            PostViewConv = stat.PostViewConv;
            Cost = stat.Cost;
            InitialMetrics = stat.InitialMetrics;
        }

        public void SetStats(IEnumerable<StatsSummary> stats)
        {
            SetBasicStats(stats);
            SetMetricStats(stats);
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

        protected void SetMetricStats(IEnumerable<StatsSummary> stats)
        {
            var allMetrics = stats.Where(x => x.InitialMetrics != null).SelectMany(x => x.InitialMetrics).ToList();
            var groupedMetrics = allMetrics.GroupBy(x => new { x.MetricType.DaysInterval, x.MetricType.Name });
            var sumMetrics = groupedMetrics.Select(x => CreateSummaryMetric(x.Key.Name, x.Key.DaysInterval, x));
            InitialMetrics = sumMetrics.ToList();
        }

        protected void AddBasicStats(StatsSummary stat)
        {
            Impressions += stat.Impressions;
            Clicks += stat.Clicks;
            AllClicks += stat.AllClicks;
            PostClickConv += stat.PostClickConv;
            PostViewConv += stat.PostViewConv;
            Cost += stat.Cost;
        }

        protected void AddMetricStats(StatsSummary stat)
        {
            var metrics = InitialMetrics == null ? new List<SummaryMetric>() : InitialMetrics.ToList();
            foreach (var metric in stat.InitialMetrics)
            {
                var sourceMetric = metrics.FirstOrDefault(x => x.MetricType.Equals(metric.MetricType));
                if (sourceMetric == null)
                {
                    metrics.Add(metric);
                }
                else
                {
                    sourceMetric.AddMetric(metric);
                }
            }

            InitialMetrics = metrics;
        }

        private SummaryMetric CreateSummaryMetric(string metricName, int? metricDaysInterval, IEnumerable<SummaryMetric> metricValues)
        {
            var metricType = new MetricType(metricName, metricDaysInterval);
            var metric = new SummaryMetric(metricType, metricValues.Sum(metrics => metrics.Value));
            return metric;
        }
    }
}