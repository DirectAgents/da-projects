using System;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;

namespace CakeExtracter.Etl.Facebook.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// The common exception for cases where something is wrong in CakeEventConversions ETL.
    /// </summary>
    public class FacebookFailedEtlException : FailedEtlException
    {
        /// <summary>
        /// Gets or sets StatsType level for which statistics was extracted.
        /// </summary>
        public string StatsType { get; set; }

        /// <inheritdoc cref="FailedEtlException"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookFailedEtlException"/> class.
        /// </summary>
        /// <param name="startDate">Start date from which statistics was extracted.</param>
        /// <param name="endDate">End date to which statistics was extracted.</param>
        /// <param name="accountId">AccountId in the database for which statistics was extracted.</param>
        /// <param name="statsType">StatsType level for which statistics was extracted.</param>
        /// <param name="innerException">The source exception.</param>
        public FacebookFailedEtlException(
            DateTime? startDate,
            DateTime? endDate,
            int? accountId,
            string statsType,
            Exception innerException)
            : base(startDate, endDate, accountId, innerException)
        {
            StatsType = statsType;
        }
    }
}