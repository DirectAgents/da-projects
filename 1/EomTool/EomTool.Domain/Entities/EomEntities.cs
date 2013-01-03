using System.Data.EntityClient;
using Ninject;

namespace EomTool.Domain.Entities
{
    public partial class EomEntities
    {
        [Inject]
        public EomEntities(IEomEntitiesConfig config)
            : this(CreateEntityConnection(config.ConnectionString))
        {
        }

        private static EntityConnection CreateEntityConnection(string databaseConnectionString)
        {
            return new EntityConnection(new EntityConnectionStringBuilder
            {
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = databaseConnectionString + ";multipleactiveresultsets=True;App=EntityFramework",
                Metadata = @"res://*/Entities.EomModel.csdl|res://*/Entities.EomModel.ssdl|res://*/Entities.EomModel.msl"
            }.ConnectionString);
        }
    }
}
