using System.Collections.Generic;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.Interfaces
{
    internal interface ISummaryMetricLoader
    {
        void AddDependentMetricTypes(IEnumerable<SummaryMetric> items);

        void AssignMetricTypeIdToItems(IEnumerable<SummaryMetric> items);

        int UpsertSummaryMetrics<TSummaryMetric>(ClientPortalProgContext db, IEnumerable<SummaryMetric> items)
            where TSummaryMetric : SummaryMetric, new();

        void RemoveMetrics<TSummaryMetric>(ClientPortalProgContext db, IEnumerable<TSummaryMetric> items)
            where TSummaryMetric : SummaryMetric, new();
    }
}
