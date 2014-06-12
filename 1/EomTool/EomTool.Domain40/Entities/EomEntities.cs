using EomTool.Domain40.Abstract;
using Ninject;
using System.Data.EntityClient;

namespace EomTool.Domain40.Entities
{
    public partial class EomEntities
    {
        [Inject]
        public EomEntities(IEomEntitiesConfig config)
            : this(CreateEntityConnection(config.ConnectionString))
        {
        }

        public EomEntities(string cs, bool dummy) // dummy ignored, used for overloading
            : this(CreateEntityConnection(cs))
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
