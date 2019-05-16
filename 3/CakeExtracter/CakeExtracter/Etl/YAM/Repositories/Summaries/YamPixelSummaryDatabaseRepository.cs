using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.YAM.Summaries;

namespace CakeExtracter.Etl.YAM.Repositories.Summaries
{
    internal class YamPixelSummaryDatabaseRepository : BaseDatabaseRepository<YamPixelSummary, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Yam Pixel Summary";

        /// <inheritdoc />
        public override object[] GetKeys(YamPixelSummary item)
        {
            return new object[] { item.Date, item.EntityId };
        }
    }
}
