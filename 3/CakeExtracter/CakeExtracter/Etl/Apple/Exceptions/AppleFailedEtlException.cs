using System;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;

namespace CakeExtracter.Etl.Apple.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// The common exception for cases where something is wrong in Apple ETL.
    /// </summary>
    public class AppleFailedEtlException : FailedEtlException
    {
        /// <summary>
        /// Gets or sets account code in the database for which statistics was extracted.
        /// </summary>
        public string AccountCode { get; set; }

        /// <inheritdoc cref="FailedEtlException"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AppleFailedEtlException"/> class.
        /// </summary>
        /// <param name="startDate">Start date from which statistics was extracted.</param>
        /// <param name="endDate">End date to which statistics was extracted.</param>
        /// <param name="accountCode">Account code in the database for which statistics was extracted.</param>
        /// <param name="innerException">The source exception.</param>
        public AppleFailedEtlException(DateTime? startDate, DateTime? endDate, string accountCode, Exception innerException)
            : base(startDate, endDate, null, innerException)
        {
            AccountCode = accountCode;
        }
    }
}
