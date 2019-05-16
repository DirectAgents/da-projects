using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.YAM;

namespace CakeExtracter.Etl.YAM.Repositories
{
    internal class YamCreativeDatabaseRepository : BaseDatabaseRepository<YamCreative, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Yam Creative";

        /// <inheritdoc />
        public override object[] GetKeys(YamCreative item)
        {
            return new object[] { item.Id };
        }
    }
}
