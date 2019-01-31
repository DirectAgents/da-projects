using System.Data.Entity;
using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.DSP;
using DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics;

namespace CakeExtracter.Etl.DSP.Loaders.ReportEntriesDataLoaders
{
    internal class LineItemsDataLoader : BaseDspItemLoader<ReportLineItem, DspLineItem, DspLineDailyMetricValues>
    {
        protected override DbSet<DspLineDailyMetricValues> GetMetricValuesDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspLineItemsMetricValues;
        }

        protected override DbSet<DspLineItem> GetVendorDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspLineItems;
        }

        protected override DspLineItem MapReportEntityToDbEntity(ReportLineItem reportEntity, ExtAccount extAccount)
        {
            return new DspLineItem
            {
                AccountId = extAccount.Id,
                Name = reportEntity.Name,
                ReportId = reportEntity.ReportId,
                AdvertiserName = reportEntity.Advertiser,
                AdvertiserReportId = reportEntity.AdvertiserReportId,
                OrderName = reportEntity.Order,
                OrderReportId = reportEntity.OrderReportId
            };
        }
    }
}
