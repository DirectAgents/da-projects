using System;

namespace CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// The common exception for cases where something is wrong in any ETL.
    /// </summary>
    public class FailedEtlException : Exception
    {
        /// <summary>
        /// Account ID in the database for which statistics was extracted.
        /// </summary>
        public int? AccountId { get; set; }

        /// <summary>
        /// Start date from which statistics was extracted.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// End date to which statistics was extracted.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="FailedEtlException"/>
        /// </summary>
        /// <param name="startDate">Start date from which statistics was extracted.</param>
        /// <param name="endDate">End date to which statistics was extracted.</param>
        /// <param name="accountId">Account ID in the database for which statistics was extracted.</param>
        /// <param name="innerException">The source exception.</param>
        public FailedEtlException(DateTime? startDate, DateTime? endDate, int? accountId, Exception innerException) : base(
            "FailedEtlException: ", innerException)
        {
            StartDate = startDate;
            EndDate = endDate;
            AccountId = accountId;
        }
    }
}
