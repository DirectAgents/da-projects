using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;

namespace EomAppModels
{
    public partial class DAMain1Entities
    {
        public static DAMain1Entities Create()
        {
            var builder = new EntityConnectionStringBuilder();

            builder.Provider = "System.Data.SqlClient";
            builder.ProviderConnectionString = string.Format("{0};multipleactiveresultsets=True;App=EntityFramework", EomAppCommon.EomAppSettings.MasterDatabaseListConnectionString);
            builder.Metadata = @"res://*/DAMain1.csdl|res://*/DAMain1.ssdl|res://*/DAMain1.msl";

            return new DAMain1Entities(builder.ConnectionString);
        }
    }

    internal static class DAMain1EntitiesExtensions
    {
        internal static DADatabase ByName(this ObjectSet<DADatabase> daDatabases, string name)
        {
            return daDatabases.FirstOrDefault(c => c.name == name);
        }
    }

    public partial class DADatabase
    {
        public static DADatabase ByName(string name)
        {
            using (var db = DAMain1Entities.Create())
            {
                return db.DADatabases.ByName(name);
            }
        }
    }
}
