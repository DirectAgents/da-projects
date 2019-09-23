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
        /// Gets or sets the account ID in the database for which statistics was extracted.
        /// </summary>
        public int? AccountId { get; set; }

        /// <summary>
        /// Gets or sets the start date from which statistics was extracted.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date to which statistics was extracted.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <inheritdoc cref="Exception"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="FailedEtlException"/> class.
        /// </summary>
        /// <param name="startDate">Start date from which statistics was extracted.</param>
        /// <param name="endDate">End date to which statistics was extracted.</param>
        /// <param name="accountId">Account ID in the database for which statistics was extracted.</param>
        /// <param name="innerException">The source exception.</param>
        public FailedEtlException(DateTime? startDate, DateTime? endDate, int? accountId, Exception innerException)
            : base("FailedEtlException: ", innerException)
        {
            StartDate = startDate;
            EndDate = endDate;
            AccountId = accountId;
        }
    }
}
