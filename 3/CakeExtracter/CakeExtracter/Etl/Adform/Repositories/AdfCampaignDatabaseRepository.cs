using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Adform;

namespace CakeExtracter.Etl.Adform.Repositories
{
    internal class AdfCampaignDatabaseRepository : BaseDatabaseRepository<AdfCampaign, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Adform Campaign";

        /// <inheritdoc />
        public override object[] GetKeys(AdfCampaign item)
        {
            return new object[] { item.Id };
        }
    }
}
