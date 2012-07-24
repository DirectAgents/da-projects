using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;

namespace EomApp1.Screens.Final.Models
{
    public partial class EomEntities
    {
        public static EomEntities Create()
        {
            return new EomEntities(new EntityConnectionStringBuilder {
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = EomAppCommon.EomAppSettings.ConnStr + ";multipleactiveresultsets=True;App=EntityFramework",
                Metadata = @"res://*/Screens.Final.Models.EomModel.csdl|res://*/Screens.Final.Models.EomModel.ssdl|res://*/Screens.Final.Models.EomModel.msl"
            }.ConnectionString);
        }
    }
}
