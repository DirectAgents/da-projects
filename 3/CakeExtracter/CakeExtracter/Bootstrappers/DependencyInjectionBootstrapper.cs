using System.ComponentModel.Composition;
using CakeExtracter.Common;

namespace CakeExtracter.Bootstrappers
{
    /// <inheritdoc />
    /// Bootstrapper for loading dependency injection modules.
    [Export(typeof(IBootstrapper))]
    internal class DependencyInjectionBootstrapper : IBootstrapper
    {
        /// <inheritdoc />
        public void Run()
        {
            DIKernel.SetKernel(new DependencyInjectionModule());
        }
    }
}
