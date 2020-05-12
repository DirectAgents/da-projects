using CakeExtracter.SimpleRepositories.BaseRepositories;

using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.MatchPortal;

namespace CakeExtracter.Common.MatchingPortal.Repositories
{
    /// <inheritdoc />
    /// <summary>
    /// BuymaHandbag Item Repository.
    /// </summary>
    public class MatchPortalRepository : BaseDatabaseRepository<BuymaHandbag, MatchPortalContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        public override string EntityName => "BuymaHandbag Item Request";

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override object[] GetKeys(BuymaHandbag item)
        {
            return new object[] { item.Level };
        }
    }
}