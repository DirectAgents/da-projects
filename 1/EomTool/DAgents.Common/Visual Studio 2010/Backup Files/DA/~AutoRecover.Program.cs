using DAgents.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.Unity;

namespace QuickBooksService
{
    class Program
    {
        static void Main(string[] args)
        {
            Init();

            string arg1 = args[0];

            IAction action = null;

            if (arg1 == "load")
                action = Locator.GetInstance<LoadAction>();
            else
                System.Console.WriteLine("no action");

            if (action != null)
                action.Execute();
        }
    }

    class LoadAction : Action
    {
        public LoadAction(QuickBooksDataLoader loader)
        {
            this.Loader = loader;
        }

        public void Execute()
        {
            Loader.Load("duh.xml");
        }

        public QuickBooksDataLoader Loader { get; set; }
    }

}
