using System;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;

namespace DAgents.Common
{
    public abstract class ConsoleProgramTemplate
    {
        public void Start(string[] args)
        {
            ConfigFile = "Config.xml";

            Init();

            Container = new UnityContainer();

            Configure();

            // ??
            //Container.BuildUp(this); 
            Logger = new ConsoleLogger();
            ConsoleHelper = new ConsoleHelper();

            //InitServiceLocator();

            try
            {
                Run();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            ConsoleHelper.WaitForKey();
        }

        //private void InitServiceLocator() { EnterpriseLibraryContainer.Current = new UnityServiceLocator(Container); }

        public dynamic Config { get; set; }
        public abstract void Init();
        public abstract void Configure();
        public abstract void Run();
        public abstract void HandleException(Exception ex);
        public virtual IUnityContainer Container { get; set; }

        //[Dependency]
        public ILogger Logger { get; set; }

        //[Dependency]
        public ConsoleHelper ConsoleHelper { get; set; }

        // todo: DI
        private string ConfigFile { set { Config = new DynamicXML(File.ReadAllText(value)); } }
    }
}
