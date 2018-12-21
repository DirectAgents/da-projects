using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA.Interfaces
{
    interface IDailySummaryLoader
    {
        void AssignIdsToItems(List<DailySummary> items);

        int UpsertDailySummaries(List<DailySummary> items);
    }
}
