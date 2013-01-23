using System;
using DAgents.Common;

namespace QuickBooksService
{
    public class LoaderDriver : ProgramAction
    {
        readonly ILoader[] Loaders;
        readonly LoaderInputReader Reader;

        public LoaderDriver(LoaderInputReader reader, ILoader[] loaders)
        {
            Reader = reader;
            Loaders = loaders;
        }

        public override void Execute()
        {
            foreach (var element in Reader)
            {
                foreach (var loader in Loaders)
                {
                    loader.Load(element);
                }
            }

            OnLoadComplete();
        }

        private void OnLoadComplete() { if (LoadComplete != null) LoadComplete(this, EventArgs.Empty); }
        public event EventHandler LoadComplete;
    }
}
