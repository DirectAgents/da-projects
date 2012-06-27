using Microsoft.Practices.Unity;

namespace DAgents.Common
{
    public interface IContainerAccessor
    {
        IUnityContainer Container { get; }
    }

    public interface IContainerAccessor<T>
    {
        T Container { get; }
    }
}
