using CakeExtracter.SimpleRepositories.BaseRepositories;

using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Facebook;

namespace CakeExtracter.Etl.Facebook.Repositories
{
    public class FacebookCampaignReachMetricDatabaseRepository : BaseDatabaseRepository<FbCampaignReachMetric, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        public override string EntityName => "Facebook Campaign Reach Metric";

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override object[] GetKeys(FbCampaignReachMetric item)
        {
            return new object[] { item.CampaignId, item.Period };
        }
    }
}