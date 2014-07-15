using ClientPortal.Data.Contracts;
using ClientPortal.Data.DTOs.TD;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Data.Entities.TD.DBM;
using System;
using System.Linq;

namespace ClientPortal.Data.Services
{
    public class TDRepository : ITDRepository, IDisposable
    {
        private TDContext context;

        public TDRepository(TDContext context)
        {
            this.context = context;
        }

        public IQueryable<DailySummary> GetDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID)
        {
            var dailySummaries = context.DailySummaries.AsQueryable();

            if (start.HasValue) dailySummaries = dailySummaries.Where(ds => ds.Date >= start.Value);
            if (end.HasValue) dailySummaries = dailySummaries.Where(ds => ds.Date <= end.Value);

            if (insertionOrderID.HasValue) dailySummaries = dailySummaries.Where(ds => ds.InsertionOrderID == insertionOrderID.Value);

            return dailySummaries;
        }

        public IQueryable<CreativeDailySummary> GetCreativeDailySummaries(DateTime? start, DateTime? end, int? insertionOrderID)
        {
            var cds = context.CreativeDailySummaries.AsQueryable();
            if (start.HasValue)
                cds = cds.Where(c => c.Date >= start.Value);
            if (end.HasValue)
                cds = cds.Where(c => c.Date <= end.Value);
            if (insertionOrderID.HasValue)
                cds = cds.Where(c => c.Creative.InsertionOrderID == insertionOrderID.Value);
            return cds;
        }

        public IQueryable<CreativeSummary> GetCreativeSummaries(DateTime? start, DateTime? end, int? insertionOrderID)
        {
            var creativeDailySummaries = GetCreativeDailySummaries(start, end, insertionOrderID);
            var creativeSummaries = creativeDailySummaries.GroupBy(cds => cds.Creative).Select(g =>
                new CreativeSummary {
                    CreativeID = g.Key.CreativeID,
                    CreativeName = g.Key.CreativeName,
                    Impressions = g.Sum(c => c.Impressions),
                    Clicks = g.Sum(c => c.Clicks),
                    Conversions = g.Sum(c => c.Conversions),
                    Revenue = g.Sum(c => c.Revenue)
                });

            return creativeSummaries;
        }

        // ---

        public IQueryable InsertionOrders()
        {
            var insertionOrders = context.InsertionOrders;
            return insertionOrders;
        }

        public InsertionOrder GetInsertionOrder(int insertionOrderID)
        {
            var insertionOrder = context.InsertionOrders.Find(insertionOrderID);
            return insertionOrder;
        }

        // ---

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
