using System.Linq;
using Ninject;
using Ninject.Modules;
using Ninject.Parameters;

namespace CakeExtracter.Common
{
    /// <summary>
    /// Utility to work with the DI mechanism.
    /// </summary>
    public static class DIKernel
    {
        private static IKernel kernel;

        /// <summary>
        /// Initializes the DI kernel by a configuration.
        /// </summary>
        /// <typeparam name="T">Type of the Ninject module, which describes the bindings between abstractions and implementations.</typeparam>
        /// <param name="module">The module.</param>
        public static void SetKernel<T>(T module)
            where T : NinjectModule
        {
            kernel = new StandardKernel(module);
        }

        /// <summary>
        /// Initializes the DI kernel by an existing kernel.
        /// </summary>
        /// <param name="sourceKernel">Existing kernel.</param>
        public static void SetKernel(IKernel sourceKernel)
        {
            kernel = sourceKernel;
        }

        /// <summary>
        /// Returns the implementation of the binding of the requested type.
        /// </summary>
        /// <typeparam name="T">The requested type for binding.</typeparam>
        /// <returns>The related type.</returns>
        public static T Get<T>()
        {
            return kernel.Get<T>();
        }

        /// <summary>
        /// Returns the implementation of the binding of the requested type.
        /// </summary>
        /// <typeparam name="T">The requested type for binding.</typeparam>
        /// <param name="constructorParameters">An array of tuples for the arguments of the constructor of the requested type.
        /// Each tuple contains a name (the corresponding name of the constructor parameter) and
        /// a value (data for the constructor parameter with the corresponding name).</param>
        /// <returns>The related type.</returns>
        public static T Get<T>(params (string name, object value)[] constructorParameters)
        {
            var commandArguments = constructorParameters.Select(x => new ConstructorArgument(x.name, x.value)).ToArray();
            return kernel.Get<T>(commandArguments);
        }
    }
}
