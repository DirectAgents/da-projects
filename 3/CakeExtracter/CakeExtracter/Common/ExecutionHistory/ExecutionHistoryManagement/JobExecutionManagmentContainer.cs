namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    public static class JobExecutionManagmentContainer
    {
        private static IJobExecutionHistoryWriter executionHistoryWriter;

        public static IJobExecutionHistoryWriter ExecutionHistoryWriter
        {
            get
            {
                if (executionHistoryWriter == null)
                {
                    var executionHistoryRepository = new JobExecutionHistoryItemRepository();
                    var executionHistoryService = new JobExecutionHistoryItemService(executionHistoryRepository);
                    executionHistoryWriter = new JobExecutionHistoryWriter(executionHistoryService);
                }
                return executionHistoryWriter;
            }
        }
    }
}
