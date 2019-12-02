using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Adform.Summaries;

namespace CakeExtracter.Etl.Adform.Repositories.Summaries
{
    internal class AdfLineItemSummaryDatabaseRepository : BaseDatabaseRepository<AdfLineItemSummary, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Adform Line Item Summary";

        /// <inheritdoc />
        public override object[] GetKeys(AdfLineItemSummary item)
        {
            return new object[] { item.Date, item.EntityId, item.MediaTypeId };
        }
    }
}
