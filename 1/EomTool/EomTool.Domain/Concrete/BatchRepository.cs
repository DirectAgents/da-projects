using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Concrete
{
    public class BatchRepository : IBatchRepository
    {
        EomEntities context;
        public BatchRepository()
        {
            context = EomEntities.Create();
        }

        public IQueryable<Batch> BatchesByBatchIds(int[] batchIds, bool includeUpdates)
        {
            IQueryable<Batch> batches;
            if (includeUpdates)
                batches = context.Batches.Include("BatchUpdates.MediaBuyerApprovalStatus");
            else
                batches = context.Batches;
            batches = batches.Where(b => batchIds.Contains(b.id));
            return batches;
        }
    }
}
