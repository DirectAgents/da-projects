using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace CakeUtility
{
    static class ContainerExtensions
    {
        internal static IUnityContainer ConfigureInterception(this IUnityContainer container)
        {
            container

                .AddNewExtension<Interception>()

            ;

            container.Configure<Interception>()

                .SetDefaultInterceptorFor<Cake.Data.Wsdl.ICakeService>(new TransparentProxyInterceptor())

                .SetDefaultInterceptorFor<DirectAgents.Common.ConcreteFactory>(new VirtualMethodInterceptor())

                .SetDefaultInterceptorFor(typeof(DirectAgents.Common.IMerger<,,>), new InterfaceInterceptor())

            ;

            return container;
        }
    }
}
