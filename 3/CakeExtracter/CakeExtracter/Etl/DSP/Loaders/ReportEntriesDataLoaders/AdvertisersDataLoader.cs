using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.DSP;
using DirectAgents.Domain.Entities.CPProg.DSP.SummaryMetrics;
using System.Data.Entity;

namespace CakeExtracter.Etl.DSP.Loaders.ReportEntriesDataLoaders
{
    /// <summary>Advertisers data loader. Loads advertisers and advertisers metric values.</summary>
    internal class AdvertisersDataLoader : BaseDspItemLoader<ReportAdvertiser, DspAdvertiser, DspAdvertiserDailyMetricValues>
    {
        /// <summary>Initializes a new instance of the <see cref="AdvertisersDataLoader"/> class.</summary>
        public AdvertisersDataLoader()
        {
        }

        /// <summary>Gets the metric values database set.</summary>
        /// <param name="dbContext">The database context.</param>
        /// <returns>Dsp entites metric values dpbset</returns>
        protected override DbSet<DspAdvertiserDailyMetricValues> GetMetricValuesDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspAdvertisersMetricValues;
        }

        /// <summary>Gets the DSP entity database set.</summary>
        /// <param name="dbContext">The database context.</param>
        /// <returns>Dsp entities dbset</returns>
        protected override DbSet<DspAdvertiser> GetDspEntityDbSet(ClientPortalProgContext dbContext)
        {
            return dbContext.DspAdvertisers;
        }

        /// <summary>Maps the report entity to database entity.</summary>
        /// <param name="reportEntity">The report entity.</param>
        /// <param name="extAccount">The ext account.</param>
        /// <returns>Initialised db entity based on report entity.</returns>
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
