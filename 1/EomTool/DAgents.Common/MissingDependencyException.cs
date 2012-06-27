using System;
namespace DAgents.Common
{
    public class MissingDependencyException : Exception
    {
        public MissingDependencyException(Type t)
            : base(t.Name)
        {
        }
    }

    public class MissingDependencyException<T> : Exception
    {
        public MissingDependencyException()
            : base(typeof(T).Name)
        {
        }
    }

    public static class MissingDependencyExceptionExtensions
    {
        public static void GuardAgainstMissingDependency<T>(this T source) where T : class
        {
            if (source == default(T))
                throw new MissingDependencyException(typeof(T));
        }
    }
}
