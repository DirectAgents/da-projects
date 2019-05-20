using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.YAM;

namespace CakeExtracter.Etl.YAM.Repositories
{
    internal class YamLineDatabaseRepository : BaseDatabaseRepository<YamLine, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Yam Line";

        /// <inheritdoc />
        public override object[] GetKeys(YamLine item)
        {
            return new object[] { item.Id };
        }
    }
}
