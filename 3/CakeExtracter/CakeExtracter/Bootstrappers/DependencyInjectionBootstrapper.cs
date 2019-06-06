using System.ComponentModel.Composition;
using CakeExtracter.Common;

namespace CakeExtracter.Bootstrappers
{
    [Export(typeof(IBootstrapper))]
    internal class DependencyInjectionBootstrapper : IBootstrapper
    {
        public void Run()
        {
            DIKernel.SetKernel<DependencyInjectionModule>();
        }
    }
}
