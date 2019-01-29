using System;

namespace DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics
{
    public class DspSummaryMetric : SummaryMetric
    {
        public DspSummaryMetric(): base()
        {
        }

        public DspSummaryMetric(int entityId, DateTime date, int metricTypeId, decimal metricValue)
        {
            EntityId = entityId;
            MetricTypeId = metricTypeId;
            Date = date;
            Value = metricValue;
        }
    }
}
