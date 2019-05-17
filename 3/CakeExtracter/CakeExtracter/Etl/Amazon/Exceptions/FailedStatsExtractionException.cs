using System;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;

namespace CakeExtracter.Etl.Amazon.Exceptions
{
    public class FailedStatsExtractionException : FailedEtlException
    {
        /// <summary>
        /// Flag for Daily dimension.
        /// </summary>
        public bool ByDaily { get; set; }

        /// <summary>
        /// Flag for Ad dimension.
        /// </summary>
        public bool ByAd { get; set; }

        /// <summary>
        /// Flag for Keyword dimension.
        /// </summary>
        public bool ByKeyword { get; set; }

        public FailedStatsExtractionException(DateTime? startDate, DateTime? endDate, int? accountId,
            Exception innerException, bool byDaily = false, bool byAd = false, bool byKeyword = false)
            : base(startDate, endDate, accountId, innerException)
        {
            ByDaily = byDaily;
            ByAd = byAd;
            ByKeyword = byKeyword;
        }
    }
}
