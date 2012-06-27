using System;
namespace DAgents.Common
{
    interface IProgramObject
    {
        Microsoft.Practices.ServiceLocation.IServiceLocator Locator { get; }
        ILogger Logger { get; set; }
    }
}
