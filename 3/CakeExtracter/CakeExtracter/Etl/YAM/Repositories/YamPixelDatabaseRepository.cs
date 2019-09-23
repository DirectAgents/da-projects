using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.YAM;

namespace CakeExtracter.Etl.YAM.Repositories
{
    internal class YamPixelDatabaseRepository : BaseDatabaseRepository<YamPixel, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Yam Pixel";

        /// <inheritdoc />
        public override object[] GetKeys(YamPixel item)
        {
            return new object[] { item.Id };
        }
    }
}
