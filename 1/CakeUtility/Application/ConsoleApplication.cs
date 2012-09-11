using System;
using System.Linq;
using System.Reflection;

namespace DirectAgents.Common
{
    public partial class ConsoleApplication
    {
        public static void Run<TProgram>(string[] args) where TProgram : new()
        {
            if (args.Length > 0)
            {
                string commandName = args[0];

                var target = from method in typeof(TProgram).GetMethods()
                             where method.Name == commandName && method.HasAttribute<CommandAttribute>()
                             select method;

                RunCommand<TProgram>(args, target.First());
            }
            else
            {
                throw new Exception("A command must be specified in order to run the application.");
            }
        }

        private static void RunCommand<TProgram>(string[] args, MethodInfo method) where TProgram : new()
        {
            TProgram instance = new TProgram();

            instance.InvokeMethod("Initialize", errorIfMethodDoesNotExist: false, accessNonPublic: true);

            if (args.Length > 1)
            {
                method.Invoke(instance, args.Skip(1).ToArray());
            }
            else
            {
                method.Invoke(instance, null);
            }
        }

        protected virtual void Initialize() { }
    }
}
