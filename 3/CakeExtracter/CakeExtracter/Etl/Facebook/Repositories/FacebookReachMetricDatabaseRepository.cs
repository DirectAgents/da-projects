using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Facebook;

namespace CakeExtracter.Etl.Facebook.Repositories
{
    internal class FacebookReachMetricDatabaseRepository : BaseDatabaseRepository<FbReachMetric, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Facebook Reach Metric";

        /// <inheritdoc />
        public override object[] GetKeys(FbReachMetric item)
        {
            return new object[] { item.AccountId, item.Period };
        }
    }
}
