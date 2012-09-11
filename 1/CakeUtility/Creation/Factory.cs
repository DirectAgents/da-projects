using System;

namespace DirectAgents.Common
{
    public abstract class Factory
    {
        public abstract T Create<T>(params object[] args);

        // Obtains concrete implementation of Factory from Locator and calls Create with the passed in args
        public static T Get<T>(params object[] args)
        {
            return Locator.Get<Factory>().Create<T>(args);
        }
    }
}
