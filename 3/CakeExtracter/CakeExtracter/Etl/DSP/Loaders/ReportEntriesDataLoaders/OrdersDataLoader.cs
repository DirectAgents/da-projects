using System.Data.Entity;
using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.DSP;
using DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics;

namespace CakeExtracter.Etl.DSP.Loaders.ReportEntriesDataLoaders
{
    internal class OrdersDataLoader : BaseDspItemLoader<ReportOrder, DspOrder, DspOrderMetricValues>
    {
        protected override DbSet<DspOrderMetricValues> GetMetricValuesDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspOrdersMetricValues;
        }

        protected override DbSet<DspOrder> GetVendorDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspOrders;
        }

        protected override DspOrder MapReportEntityToDbEntity(ReportOrder reportEntity, ExtAccount extAccount)
        {
            return new DspOrder
            {
                AccountId = extAccount.Id,
                Name = reportEntity.Name,
                ReportId = reportEntity.ReportId,
                AdvertiserName = reportEntity.Advertiser,
                AdvertiserReportId = reportEntity.AdvertiserReportId
            };
        }
    }
}
