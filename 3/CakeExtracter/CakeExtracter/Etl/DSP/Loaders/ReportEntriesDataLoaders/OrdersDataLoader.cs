using System.Data.Entity;
using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.DSP;
using DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics;

namespace CakeExtracter.Etl.DSP.Loaders.ReportEntriesDataLoaders
{
    /// <summary>orders entities and metric values db set</summary>
    internal class OrdersDataLoader : BaseDspItemLoader<ReportOrder, DspOrder, DspOrderMetricValues>
    {
        protected override DbSet<DspOrderMetricValues> GetMetricValuesDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspOrdersMetricValues;
        }

        /// <summary>Gets the DSP entity database set.</summary>
        /// <param name="dbContext">The database context.</param>
        /// <returns>Dsp entities dbset</returns>
        protected override DbSet<DspOrder> GetDspEntityDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspOrders;
        }

        /// <summary>Maps the report entity to database entity.</summary>
        /// <param name="reportEntity">The report entity.</param>
        /// <param name="extAccount">The ext account.</param>
        /// <returns>Initialised db entity based on report entity.</returns>
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
