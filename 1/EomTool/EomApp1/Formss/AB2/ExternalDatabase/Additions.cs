using System.Configuration;
using System.Data.EntityClient;

namespace EomApp1.Formss.AB2.ExternalDatabase
{
    public partial class ExternalDatabaseModel
    {
        static public ExternalDatabaseModel CreateExternalDatabaseModel(string sqlServerConnectionString)
        {
            ExternalDatabaseModel result;

            var connectionString = ConfigurationManager.ConnectionStrings["ExternalDatabaseModel"];
            var entityConnectionStringBuilder = new EntityConnectionStringBuilder(connectionString.ConnectionString);
            entityConnectionStringBuilder.ProviderConnectionString = sqlServerConnectionString.TrimEnd(';') + "; multipleactiveresultsets=True; App=EntityFramework";

            result = new ExternalDatabaseModel(entityConnectionStringBuilder.ToString());
            return result;
        }
    }
}
