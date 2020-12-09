using CakeExtracter.SimpleRepositories.BaseRepositories;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Adform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.Adform.Repositories
{
    public class AdfTrackingPointDatabaseRepository : BaseDatabaseRepository<AdfTrackingPoint, ClientPortalProgContext>
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        protected override object Locker { get; set; } = RequestLocker;

        /// <inheritdoc />
        public override string EntityName => "Adform LineItem";

        /// <inheritdoc />
        public override object[] GetKeys(AdfTrackingPoint item)
        {
            return new object[] { item.Id };
        }
    }
}
