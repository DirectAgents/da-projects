using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace CakeExtracter.Data.Configurations
{
    public class AzureConfiguration : DbConfiguration
    {
        public AzureConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new DaExecutionStrategy());
            SetDefaultConnectionFactory(new LocalDbConnectionFactory("mssqllocaldb"));
        }
    }
}
