using System.Data.EntityClient;
using EomAppCommon;

namespace EomApp1.Screens.Final.Models
{
    public partial class Eom
    {
        public static Eom Create()
        {
            return new Eom(new EntityConnectionStringBuilder {
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = EomAppSettings.ConnStr + ";multipleactiveresultsets=True;App=EntityFramework",
                Metadata = @"res://*/Screens.Final.Models.EomModel.csdl|res://*/Screens.Final.Models.EomModel.ssdl|res://*/Screens.Final.Models.EomModel.msl"
            }.ConnectionString);
        }
    }
}
