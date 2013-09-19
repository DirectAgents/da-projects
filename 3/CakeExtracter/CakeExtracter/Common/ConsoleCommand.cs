using System.Collections.Generic;
using System.Reflection;

namespace CakeExtracter.Common
{
    public abstract class ConsoleCommand : ManyConsole.ConsoleCommand
    {
        private List<ConsoleCommand> commandsToRunBeforeThisCommand = new List<ConsoleCommand>();

        protected void RunBefore(ConsoleCommand consoleCommand)
        {
            this.commandsToRunBeforeThisCommand.Add(consoleCommand);
        }

        public override int Run(string[] remainingArguments)
        {
            foreach (var consoleCommand in this.commandsToRunBeforeThisCommand)
            {
                Logger.Info("Running before command: {0}..", consoleCommand.GetType().Name);
                consoleCommand.Run(null);
            }

            using (new LogElapsedTime())
            {
                var retCode = Execute(remainingArguments);
                return retCode;
            }
        }

        public abstract int Execute(string[] remainingArguments);

        public virtual IEnumerable<PropertyInfo> GetArgumentProperties()
        {
            return this.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
        }

        public virtual bool TrySetProperty(string propertyName, object propertyValue)
        {
            var property = this.GetType().GetProperty(propertyName);
            if (property != null)
            {
                property.SetValue(this, propertyValue);
                return true;
            }
            return false;
        }


    }
}
