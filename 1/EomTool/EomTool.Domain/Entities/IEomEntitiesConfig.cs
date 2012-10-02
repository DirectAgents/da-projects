using System;
namespace EomTool.Domain.Entities
{
    public interface IEomEntitiesConfig
    {
        string ConnectionStringByDate(DateTime eomDate);
        DateTime CurrentEomDate { get; set; }
    }
}
