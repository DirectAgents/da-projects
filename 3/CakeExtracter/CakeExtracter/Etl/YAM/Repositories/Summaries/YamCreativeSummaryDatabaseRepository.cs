using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Repositories.Summaries
{
    internal class YamCreativeSummaryDatabaseRepository : BaseDatabaseRepository<YamCreativeSummary, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Yam Creative Summary";

        /// <inheritdoc />
        public override object[] GetKeys(YamCreativeSummary item)
        {
            return new object[] { item.Date, item.EntityId };
        }
    }
}
