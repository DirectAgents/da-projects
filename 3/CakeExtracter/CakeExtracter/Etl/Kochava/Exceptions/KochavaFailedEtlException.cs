using System;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;

namespace CakeExtracter.Etl.Kochava.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// The common exception for cases where something is wrong in Kochava ETL.
    /// </summary>
    public class KochavaFailedEtlException : FailedEtlException
    {

        /// <inheritdoc cref="FailedEtlException"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="KochavaFailedEtlException"/> class.
        /// </summary>
        /// <param name="startDate">Start date from which statistics was extracted.</param>
        /// <param name="endDate">End date to which statistics was extracted.</param>
        /// <param name="accountId">Account Id in the database for which statistics was extracted.</param>
        /// <param name="innerException">The source exception.</param>
        public KochavaFailedEtlException(DateTime? startDate, DateTime? endDate, int accountId,
            Exception innerException)
            : base(startDate, endDate, accountId, innerException)
        {
        }
    }
}
