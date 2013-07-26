using System;
using System.Linq;
using System.Runtime.Remoting;
using CakeExtracter.Common;
using Quartz;

namespace CakeExtracter.Commands
{
    public class ScheduledCommandJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var jobDataMap = context.JobDetail.JobDataMap;
                var typeName = (string)jobDataMap["typeName"];
                var assemblyName = (string)jobDataMap["assemblyName"];
                ObjectHandle handle = Activator.CreateInstance(assemblyName, typeName);
                var consoleCommand = (ConsoleCommand)handle.Unwrap();
                var propertyPrefix = "property.";
                foreach (var item in jobDataMap.Where(c => c.Key.StartsWith(propertyPrefix))
                                               .Select(c => new
                                               {
                                                   PropertyName = c.Key.Substring(propertyPrefix.Length),
                                                   PropertyValue = c.Value
                                               }))
                {
                    consoleCommand.GetType().GetProperty(item.PropertyName).SetValue(consoleCommand, item.PropertyValue);
                }
                Logger.Info("Executing ScheduledCommandJob: " + consoleCommand.Command);
                consoleCommand.Run(null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
