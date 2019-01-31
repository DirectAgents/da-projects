using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.DSP;
using DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics;
using System.Data.Entity;

namespace CakeExtracter.Etl.DSP.Loaders.ReportEntriesDataLoaders
{
    internal class AdvertisersDataLoader : BaseDspItemLoader<ReportAdvertiser, DspAdvertiser, DspAdvertiserDailyMetricValues>
    {
        public AdvertisersDataLoader()
        {
        }

        protected override DbSet<DspAdvertiserDailyMetricValues> GetMetricValuesDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspAdvertisersMetricValues;
        }

        protected override DbSet<DspAdvertiser> GetVendorDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspAdvertisers;
        }

        protected override DspAdvertiser MapReportEntityToDbEntity(ReportAdvertiser reportEntity, ExtAccount extAccount)
        {
            return new DspAdvertiser
            {
                AccountId = extAccount.Id,
                Name = reportEntity.Name,
                ReportId = reportEntity.ReportId,
            };
        }
    }
}
