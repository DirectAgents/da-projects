using ClientPortal.Data.DTOs.TD;
using ClientPortal.Data.Entities.TD.DBM;
using System;
using System.Linq;

namespace ClientPortal.Data.Contracts
{
    public interface ITDRepository : IDisposable
    {
        IQueryable<DailySummary> GetDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID);
        IQueryable<CreativeDailySummary> GetCreativeDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID);
        IQueryable<CreativeSummary> GetCreativeSummaries(DateTime? start, DateTime? end, int? insertionOrderID);

        IQueryable<InsertionOrder> InsertionOrders();
        InsertionOrder GetInsertionOrder(int insertionOrderID);
    }
}
