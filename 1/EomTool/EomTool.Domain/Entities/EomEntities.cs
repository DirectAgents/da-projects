using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;

namespace EomTool.Domain.Entities
{
    public partial class EomEntities
    {
        public static EomEntities Create()
        {
            //TODO: unhardcode!
            GlobalSettings.ConnStr = @"data source=biz2\da;initial catalog=zDADatabaseJuly2012Test2;integrated security=True";

            return new EomEntities(new EntityConnectionStringBuilder
            {
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = GlobalSettings.ConnStr + ";multipleactiveresultsets=True;App=EntityFramework",
                Metadata = @"res://*/Entities.EomModel.csdl|res://*/Entities.EomModel.ssdl|res://*/Entities.EomModel.msl"
            }.ConnectionString);
        }
    }
}
