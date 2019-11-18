using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Repositories.Summaries
{
    internal class AdfBannerSummaryDatabaseRepository : BaseDatabaseRepository<AdfBannerSummary, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Adform Banner Summary";

        /// <inheritdoc />
        public override object[] GetKeys(AdfBannerSummary item)
        {
            return new object[] { item.Id };
        }
    }
}
