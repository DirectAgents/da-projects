using System.Collections.Generic;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Abstract
{
    public interface IDAMain1Repository
    {
        List<DADatabase> DADatabases { get; }
    }
}
