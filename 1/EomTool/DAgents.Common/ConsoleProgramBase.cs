using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;

namespace DAgents.Common
{
    /// <summary>
    /// Abstract base for a command line application.
    /// </summary>
    public abstract class ConsoleProgramBase
    {
        private string ConfigFile { set { Config = new DynamicXML(File.ReadAllText(value)); } }

        /// <summary>
        ///
        /// </summary>
        public ConsoleProgramBase(ILogger logger = null)
        {
            this.ConfigFile = "Config.xml";

            this.UnityContainer = new UnityContainer();

            this.Configure();

            this.Logger = logger ?? new ConsoleLogger();

            EnterpriseLibraryContainer.Current = new UnityServiceLocator(UnityContainer);
        }

        /// <summary>
        /// 
        /// </summary>
        public dynamic Config { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public abstract void Configure();

        /// <summary>
        /// 
        /// </summary>
        public abstract IUnityContainer UnityContainer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Dependency]
        public ILogger Logger { get; set; } 
    }
}
