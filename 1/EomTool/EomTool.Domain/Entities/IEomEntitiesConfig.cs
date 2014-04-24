using System;
namespace EomTool.Domain.Entities
{
    public interface IEomEntitiesConfig
    {
        string ConnectionString { get; }
        DateTime CurrentEomDate { get; set; }
        bool DebugMode { get; }
    }
}
