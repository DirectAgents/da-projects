using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;
using System;

namespace CakeExtracter.Etl.DSP.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// The common exception for cases where something is wrong in AmazonDsp ETL.
    /// </summary>
    public class DspFailedEtlException : FailedEtlException
    {
        /// <inheritdoc cref="FailedEtlException"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="DspFailedEtlException"/> class.
        /// </summary>
        /// <param name="startDate">Start date from which statistics was extracted.</param>
        /// <param name="endDate">End date to which statistics was extracted.</param>
        /// <param name="accountId">AccountId in the database for which statistics was extracted.</param>
        /// <param name="innerException">The source exception.</param>
        public DspFailedEtlException(
            DateTime? startDate,
            DateTime? endDate,
            int? accountId,
            Exception innerException)
            : base(startDate, endDate, accountId, innerException)
        {
        }
    }
}
