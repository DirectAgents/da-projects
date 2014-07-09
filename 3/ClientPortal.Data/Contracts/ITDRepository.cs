using ClientPortal.Data.Entities.TD;
using System;
using System.Linq;

namespace ClientPortal.Data.Contracts
{
    public interface ITDRepository : IDisposable
    {
        IQueryable<DailySummary> GetDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID);
    }
}
