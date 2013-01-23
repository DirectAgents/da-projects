using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountingBackupWeb.Models.EOM
{
    public class EomModel : IEomModel
    {
        public EomDatabaseEntities GetContainer(string serverName, string databaseName)
        {
            var factory = new EntityConnectionFactory<EomDatabaseEntities>(serverName, databaseName, "EOM.EomDatabaseModel");
            var result = factory.Create();
            return result;
        }
    }
}
