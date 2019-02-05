using System;

namespace DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics
{
    public class BaseDailyMetricValues
    {
        public int EntityId { get; set; }

        public DateTime Date { get; set; }

        public BaseDailyMetricValues(): base()
        {
        }

        public BaseDailyMetricValues(int entityId, DateTime date)
        {
            EntityId = entityId;
            Date = date;
        }
    }
}
