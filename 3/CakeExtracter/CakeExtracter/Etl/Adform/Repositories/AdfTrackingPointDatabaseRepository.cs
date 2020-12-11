using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Adform;

namespace CakeExtracter.Etl.Adform.Repositories
{
    public class AdfTrackingPointDatabaseRepository : BaseDatabaseRepository<AdfTrackingPoint, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        public override string EntityName => "Adform Tracking Point";

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override object[] GetKeys(AdfTrackingPoint item)
        {
            return new object[] { item.Id };
        }
    }
}
