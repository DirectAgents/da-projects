using System;
namespace DAgents.Common
{
    public interface IServiceLocator
    {
        T Resolve<T>();
    }
}
