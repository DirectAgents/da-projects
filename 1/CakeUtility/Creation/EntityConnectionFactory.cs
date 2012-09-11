using System.Data.EntityClient;
using System.Data.SqlClient;

namespace DirectAgents.Common
{
    public class EntityConnectionFactory : IFactory<EntityConnection>
    {      
        private string server;
        private string catalog;
        private string metaData;
        private string dataProvider;

        public EntityConnectionFactory(string sqlServerName, string databaseName, string metaData, string dataProvider = "System.Data.SqlClient")
        {
            this.server = sqlServerName;
            this.catalog = databaseName;
            this._Metadata = metaData;
            this.dataProvider = dataProvider;
        }

        public EntityConnection Create()
        {
            return DoCreate();
        }

        private EntityConnection DoCreate()
        {
            var sqlBuilder = new SqlConnectionStringBuilder {
                DataSource = server,
                InitialCatalog = catalog,
                IntegratedSecurity = true,
            };

            var entityBuilder = new EntityConnectionStringBuilder {
                Provider = dataProvider,
                ProviderConnectionString = sqlBuilder.ToString(),
                Metadata = _Metadata,
            };

            var entityConnection = new EntityConnection(entityBuilder.ConnectionString);

            return entityConnection;
        }

        private string _Metadata
        {
            get
            {
                // Option 1: set the full metadata
                // Option 2: use the name that gets pushed into the format string below
                string metadata;
                if (!this.metaData.StartsWith("res://"))
                {
                    metadata = string.Format(@"res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl", this.metaData);
                }
                else
                {
                    metadata = this.metaData;
                }
                return metadata;
            }
            set
            {
                this.metaData = value;
            }
        }
    }
}
