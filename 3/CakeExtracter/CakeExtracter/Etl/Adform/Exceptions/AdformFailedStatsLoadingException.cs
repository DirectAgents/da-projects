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
        /// Gets or sets a value indicating whether exception for the Daily dimension.
        /// </summary>
        public bool ByDaily { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether exception for the Campaign dimension.
        /// </summary>
        public bool ByCampaign { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether exception for the Line Item dimension.
        /// </summary>
        public bool ByLineItem { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether exception for the Banner dimension.
        /// </summary>
        public bool ByBanner { get; set; }

        /// <inheritdoc cref="FailedEtlException"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AdformFailedStatsLoadingException" /> class.
        /// </summary>
        public AdformFailedStatsLoadingException(
            DateTime? startDate,
            DateTime? endDate,
            int? accountId,
            Exception innerException,
            bool byDaily = false,
            bool byCampaign = false,
            bool byLineItem = false,
            bool byBanner = false)
            : base(startDate, endDate, accountId, innerException)
        {
            ByDaily = byDaily;
            ByCampaign = byCampaign;
            ByLineItem = byLineItem;
            ByBanner = byBanner;
        }
    }
}
