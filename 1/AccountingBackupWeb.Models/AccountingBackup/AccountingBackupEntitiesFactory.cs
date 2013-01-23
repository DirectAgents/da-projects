using System.Data.EntityClient;
using System.Data.SqlClient;

namespace AccountingBackupWeb.Models.AccountingBackup
{
    public class AccountingBackupEntitiesFactory : AccountingBackupEntities.IFactory
    {
        readonly string _server;
        readonly string _database;
        readonly string _provider;

        public AccountingBackupEntitiesFactory(string server, string database, string provider)
        {
            _server = server;
            _database = database;
            _provider = provider;
        }

        public AccountingBackupEntities Create()
        {
            return new AccountingBackupEntities(CreateEntityConnection());
        }

        EntityConnection CreateEntityConnection()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = _server;
            sqlBuilder.InitialCatalog = _database;
            sqlBuilder.IntegratedSecurity = true;
            string providerString = sqlBuilder.ToString();

            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
            entityBuilder.Provider = _provider;
            entityBuilder.ProviderConnectionString = providerString;
            entityBuilder.Metadata = @"res://*/AccountingBackup.AccountingBackupData.csdl|res://*/AccountingBackup.AccountingBackupData.ssdl|res://*/AccountingBackup.AccountingBackupData.msl";

            return new EntityConnection(entityBuilder.ConnectionString);
        }
    }
}
