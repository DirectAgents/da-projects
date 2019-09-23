namespace DirectAgents.Domain.Entities.Administration.JobExecution.Enums
{
    /// <summary>
    /// The status of the job execution.
    /// </summary>
    public enum JobExecutionStatus
    {
        Processing = 0,
        Completed = 1,
        Failed = 2,
        Aborted = 3,
        AbortedByTimeout = 4,
    }
}
