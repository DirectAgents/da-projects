namespace CakeExtracter.Common.ExecutionHistory.ExecutionHistoryManagement
{
    public static class JobExecutionManagmentContainer
    {
        private static JobExecutionHistoryWriter executionHistoryWriter;

        public static JobExecutionHistoryWriter ExecutionHistoryWriter
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
