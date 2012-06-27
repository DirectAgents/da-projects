using System;
using System.Collections.Generic;
namespace EomApp1.Formss.AB2.Model
{
    public interface ISqlServerDatabaseSource
    {
        IEnumerable<SqlServerDatabase> SqlServerDatabases { get; }
    }
}
