using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Adform;

namespace CakeExtracter.Etl.Adform.Repositories
{
    internal class AdfLineItemDatabaseRepository : BaseDatabaseRepository<AdfLineItem, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Adform Line Item";

        /// <inheritdoc />
        public override object[] GetKeys(AdfLineItem item)
        {
            return new object[] { item.Id };
        }
    }
}
