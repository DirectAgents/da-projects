using System;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions
{
    public class FailedEtlException : Exception
    {
        public int AccountId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public FailedEtlException(DateTime startDate, DateTime endDate, int accountId, Exception innerException) : base(
            "FailedEtlException: ", innerException)
        {
            StartDate = startDate;
            EndDate = endDate;
            AccountId = accountId;
        }
    }
}
