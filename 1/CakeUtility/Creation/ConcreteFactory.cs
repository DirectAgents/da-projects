using System.Linq;
using System.Reflection;

namespace DirectAgents.Common
{
    using Aspects;

    [NotThreadSafe]
    public class ConcreteFactory : Factory
    {
        [CountCalls]
        public override T Create<T>(params object[] args)
        {
            ConstructorInfo constructor = typeof(T).GetConstructor(args.Select(c => c.GetType()).ToArray());
            T newObject = (T)constructor.Invoke(args);
            return newObject;
        }
    }
}
