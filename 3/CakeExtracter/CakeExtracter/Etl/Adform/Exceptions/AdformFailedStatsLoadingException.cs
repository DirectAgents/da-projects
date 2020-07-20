using System;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;

namespace CakeExtracter.Etl.Adform.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// The exception for cases where something is wrong in Adform ETL.
    /// </summary>
    public class AdformFailedStatsLoadingException : FailedEtlException
    {
        /// <summary>
        /// Gets or sets StatsType level for which statistics was extracted.
        /// </summary>
        public string StatsType { get; set; }

        /// <inheritdoc cref="FailedEtlException"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AdformFailedStatsLoadingException" /> class.
        /// </summary>
        public AdformFailedStatsLoadingException(
            DateTime? startDate,
            DateTime? endDate,
            int? accountId,
            Exception innerException,
            string statsType)
            : base(startDate, endDate, accountId, innerException)
        {
            StatsType = statsType;
        }
    }
}
