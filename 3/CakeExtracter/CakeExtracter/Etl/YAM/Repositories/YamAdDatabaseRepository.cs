using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.YAM;

namespace CakeExtracter.Etl.YAM.Repositories
{
    internal class YamAdDatabaseRepository : BaseDatabaseRepository<YamAd, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Yam Ad";

        /// <inheritdoc />
        public override object[] GetKeys(YamAd item)
        {
            return new object[] { item.Id };
        }
    }
}
