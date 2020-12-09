using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Repositories.Summaries
{
    public class AdfTrackingPointSummaryDatabaseRepository : BaseDatabaseRepository<AdfTrackingPointSummary, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Adform Tracking Point Summary";

        /// <inheritdoc />
        public override object[] GetKeys(AdfTrackingPointSummary item)
        {
            return new object[] { item.Date, item.EntityId, item.MediaTypeId };
        }
    }
}
