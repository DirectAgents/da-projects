using System;

namespace EomTool.Domain40.Abstract
{
    public interface IEomEntitiesConfig
    {
        string ConnectionString { get; }
        DateTime CurrentEomDate { get; set; }
        string CurrentEomDateString { get; }
        bool DebugMode { get; }
    }
}
