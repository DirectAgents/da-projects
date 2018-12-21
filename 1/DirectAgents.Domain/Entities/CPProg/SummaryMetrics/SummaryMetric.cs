using System;

namespace DirectAgents.Domain.Entities.CPProg
{
    public class SummaryMetric
    {
        public int EntityId { get; set; }
        public DateTime Date { get; set; }

        public int MetricTypeId { get; set; }
        public virtual MetricType MetricType { get; set; }

        public decimal Value { get; set; }

        public SummaryMetric()
        {
        }

        public SummaryMetric(DateTime date, MetricType metricType, decimal metricValue)
        {
            Date = date;
            MetricType = metricType;
            Value = metricValue;
        }

        public SummaryMetric(MetricType metricType, decimal metricValue)
        {
            MetricType = metricType;
            Value = metricValue;
        }

        public void AddMetric(SummaryMetric metric)
        {
            if (MetricTypeId == metric.MetricTypeId || MetricType.Equals(metric.MetricType))
            {
                Value += metric.Value;
            }
        }
    }
}