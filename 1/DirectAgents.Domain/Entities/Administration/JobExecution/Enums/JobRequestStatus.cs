namespace DirectAgents.Domain.Entities.Administration.JobExecution.Enums
{
    /// <summary>
    /// The status of the job request.
    /// </summary>
    public enum JobRequestStatus
    {
        Scheduled = 0,
        Processing = 1,
        Completed = 2,
        Failed = 3,
        StartedByAnotherRequest = 4,
        Aborted = 5,
    }
}
