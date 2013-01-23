using DAgents.Common;
using Microsoft.Practices.Unity;

namespace QuickBooksService
{
    class MyContainer : IContainerAccessor<IUnityContainer>
    {
        public MyContainer(IUnityContainer uc, ILogger logger)
        {
            Container = uc.CreateChildContainer();

            Container
                .RegisterInstance<ILogger>(logger ?? new ConsoleLogger())
            ;
        }

        public IUnityContainer Container { get; set; }
    }
}
