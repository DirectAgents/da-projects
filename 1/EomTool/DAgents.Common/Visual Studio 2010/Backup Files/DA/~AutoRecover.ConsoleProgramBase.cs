using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace DAgents.Common
{
    public class ConsoleProgramBase
    {
        protected ConsoleProgramBase()
        {
            // Create the container
            Container = new UnityContainer();

            RegisterTypes();

            Container.RegisterType<ILogger, ConsoleLogger>();

            // Wrap in ServiceLocator
            Locator = new UnityServiceLocator(Container);

            // And set Enterprise Library to use it
            EnterpriseLibraryContainer.Current = Locator;
        }

        private static void RegisterTypes()
        {
            throw new NotImplementedException();
        }

        protected static IUnityContainer Container;

        public static Microsoft.Practices.ServiceLocation.IServiceLocator Locator { get; set; }

        [Dependency]
        public ILogger Logger { get; set; }
    }
}
