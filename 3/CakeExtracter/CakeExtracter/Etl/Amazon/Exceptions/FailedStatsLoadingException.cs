using System;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;

namespace CakeExtracter.Etl.Amazon.Exceptions
{
    public class FailedStatsLoadingException : FailedEtlException
    {
        /// <summary>
        /// Flag for Daily dimension.
        /// </summary>
        public bool ByDaily { get; set; }

        /// <summary>
        /// Flag for Campaign dimension.
        /// </summary>
        public bool ByCampaign { get; set; }

        /// <summary>
        /// Flag for Ad dimension.
        /// </summary>
        public bool ByAd { get; set; }

        /// <summary>
        /// Flag for Keyword dimension.
        /// </summary>
        public bool ByKeyword { get; set; }

        /// <summary>
        /// Flag for Search Term dimension.
        /// </summary>
        public bool BySearchTerm { get; set; }

        public FailedStatsLoadingException(
            DateTime? startDate,
            DateTime? endDate,
            int? accountId,
            Exception innerException,
            bool byDaily = false,
            bool byAd = false,
            bool byKeyword = false,
            bool byCampaign = false,
            bool bySearchTerm = false)
            : base(startDate, endDate, accountId, innerException)
        {
            ByDaily = byDaily;
            ByAd = byAd;
            ByKeyword = byKeyword;
            ByCampaign = byCampaign;
            BySearchTerm = bySearchTerm;
        }
    }
}
