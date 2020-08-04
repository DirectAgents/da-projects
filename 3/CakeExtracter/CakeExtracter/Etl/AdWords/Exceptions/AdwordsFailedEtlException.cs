using System;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;

namespace CakeExtracter.Etl.AdWords.Exceptions
{
    internal class AdwordsFailedEtlException : FailedEtlException
    {
        /// <summary>
        /// Gets or sets client id in the database for which statistics was extracted.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets StatsType level for which statistics was extracted.
        /// </summary>
        public string StatsType { get; set; }

        /// <inheritdoc cref="FailedEtlException"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AdwordsFailedEtlException"/> class.
        /// </summary>
        /// <param name="startDate">Start date from which statistics was extracted.</param>
        /// <param name="endDate">End date to which statistics was extracted.</param>
        /// <param name="clientId">Client in the database for which statistics was extracted.</param>
        /// <param name="statsType">StatsType level for which statistics was extracted.</param>
        /// <param name="innerException">The source exception.</param>
        public AdwordsFailedEtlException(
            DateTime? startDate,
            DateTime? endDate,
            string clientId,
            string statsType,
            Exception innerException)
                : base(startDate, endDate, null, innerException)
        {
            ClientId = clientId;
            StatsType = statsType;
        }
    }
}
