using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Repositories.Summaries
{
    internal class YamLineSummaryDatabaseRepository : BaseDatabaseRepository<YamLineSummary, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Yam Line Summary";

        /// <inheritdoc />
        public override object[] GetKeys(YamLineSummary item)
        {
            return new object[] { item.Date, item.EntityId };
        }
    }
}
