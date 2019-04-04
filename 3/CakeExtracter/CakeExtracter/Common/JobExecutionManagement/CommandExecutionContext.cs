using CakeExtracter.Common.JobExecutionManagement.JobExecution;
using System;

namespace CakeExtracter.Common.JobExecutionManagement
{
    public class CommandExecutionContext
    {
        public JobExecutionDataWriter JobDataWriter
        {
            get;
            private set;
        }

        private CommandExecutionContext()
        {
            var executionItemRepository = new JobExecutionItemRepository();
            var executionItemService = new JobExecutionItemService(executionItemRepository);
            this.JobDataWriter = new JobExecutionDataWriter(executionItemService);
        }

        public static CommandExecutionContext Current;
        
        public static void InitContext()
        {
            if (Current != null)
            {
                throw new Exception("Execution context already initialised.");
            }
            else
            {
                Current = new CommandExecutionContext();
            }
            
        }
    }
}
