using System;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;

namespace CakeExtracter.Etl.CakeMarketing.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// The common exception for cases where something is wrong in CakeEventConversions ETL.
    /// </summary>
    public class CakeEventConversionsFailedEtlException : FailedEtlException
    {
        /// <summary>
        /// Gets or sets AdvertiserId in the database for which statistics was extracted.
        /// </summary>
        public int? AdvertiserId { get; set; }

        /// <summary>
        /// Gets or sets OfferId in the database for which statistics was extracted.
        /// </summary>
        public int? OfferId { get; set; }

        /// <inheritdoc cref="FailedEtlException"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="CakeEventConversionsFailedEtlException"/> class.
        /// </summary>
        /// <param name="startDate">Start date from which statistics was extracted.</param>
        /// <param name="endDate">End date to which statistics was extracted.</param>
        /// <param name="advertiserId">AdvertiserId in the database for which statistics was extracted.</param>
        /// <param name="offerId">OfferId in the database for which statistics was extracted.</param>
        /// <param name="innerException">The source exception.</param>
        public CakeEventConversionsFailedEtlException(DateTime? startDate, DateTime? endDate, int? advertiserId, int? offerId, Exception innerException)
            : base(startDate, endDate, null, innerException)
        {
            AdvertiserId = advertiserId;
            OfferId = offerId;
        }
    }
}