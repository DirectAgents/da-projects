using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountingBackupWeb.Models.EOM
{
    public interface IEomModel
    {
        EomDatabaseEntities GetContainer(string serverName, string databaseName);
    }
}
