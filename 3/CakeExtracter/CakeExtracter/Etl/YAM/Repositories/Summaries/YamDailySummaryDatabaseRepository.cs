using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Repositories.Summaries
{
    internal class YamDailySummaryDatabaseRepository : BaseDatabaseRepository<YamDailySummary, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Yam Daily Summary";

        /// <inheritdoc />
        public override object[] GetKeys(YamDailySummary item)
        {
            return new object[] { item.Date, item.EntityId };
        }
    }
}
