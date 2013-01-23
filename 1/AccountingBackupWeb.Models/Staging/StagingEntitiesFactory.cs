using System.Data.EntityClient;
using System.Data.SqlClient;

namespace AccountingBackupWeb.Models.Staging
{
    public class StagingEntitesFactory : StagingEntities.IFactory
    {
        readonly string _server;
        readonly string _database;
        readonly string _provider;

        public StagingEntitesFactory(string server, string database, string provider)
        {
            _server = server;
            _database = database;
            _provider = provider;
        }

        public StagingEntities Create()
        {
            return new StagingEntities(EntityConnection());
        }

        // todo: dup
        EntityConnection EntityConnection()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = _server;
            sqlBuilder.InitialCatalog = _database;
            sqlBuilder.IntegratedSecurity = true;
            string providerString = sqlBuilder.ToString();

            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
            entityBuilder.Provider = _provider;
            entityBuilder.ProviderConnectionString = providerString;
            entityBuilder.Metadata = @"res://*/Staging.Staging.csdl|res://*/Staging.Staging.ssdl|res://*/Staging.Staging.msl";

            return new EntityConnection(entityBuilder.ConnectionString);
        }
    }
}
