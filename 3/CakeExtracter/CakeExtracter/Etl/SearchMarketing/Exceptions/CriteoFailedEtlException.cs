using System;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;

namespace CakeExtracter.Etl.Criteo.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// The common exception for cases where something is wrong in Criteo ETL.
    /// </summary>
    public class CriteoFailedEtlException : FailedEtlException
    {
        /// <summary>
        /// Gets or sets the Stats Type in the database for which statistics was extracted.
        /// </summary>
        public string StatsType;

        /// <inheritdoc cref="FailedEtlException"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="CriteoFailedEtlException"/> class.
        /// </summary>
        /// <param name="startDate">Start date from which statistics was extracted.</param>
        /// <param name="endDate">End date to which statistics was extracted.</param>
        /// <param name="accountId">Account Id in the database for which statistics was extracted.</param>
        /// <param name="innerException">The source exception.</param>
        public CriteoFailedEtlException(DateTime? startDate, DateTime? endDate, int accountId, Exception innerException, string statsType)
            : base(startDate, endDate, accountId, innerException)
        {
            StatsType = statsType;
        }
    }
}
