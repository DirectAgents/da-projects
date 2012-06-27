using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;

namespace DAgents.Common
{
    public class ProgramObject : IProgramObject
    {
        [Dependency]
        public ILogger Logger { get; set; }

        public Microsoft.Practices.ServiceLocation.IServiceLocator Locator
        {
            get { return EnterpriseLibraryContainer.Current; }
        }
    }
}
