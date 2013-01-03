using System;
namespace EomTool.Domain.Entities
{
    public interface IEomEntitiesConfig
    {
        DateTime CurrentEomDate { get; set; }
        string ConnectionString { get; }
    }
}
