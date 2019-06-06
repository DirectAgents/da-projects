using System.Linq;
using Ninject;
using Ninject.Modules;
using Ninject.Parameters;

namespace CakeExtracter.Common
{
    public static class DIKernel
    {
        private static IKernel kernel;

        public static void SetKernel<T>()
            where T : NinjectModule, new()
        {
            var module = new T();
            kernel = new StandardKernel(module);
        }

        public static T Get<T>()
        {
            return kernel.Get<T>();
        }

        public static T Get<T>(params (string name, object value)[] constructorParameters)
        {
            var commandArguments = constructorParameters.Select(x => new ConstructorArgument(x.name, x.value)).ToArray();
            return kernel.Get<T>(commandArguments);
        }
    }
}
