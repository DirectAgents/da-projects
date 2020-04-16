using System;

namespace CakeExtracter.Etl.CakeMarketing.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// The common exception for cases where something is wrong in CakeAffSubSums ETL.
    /// </summary>
    public class CakeAffSubSumsFailedEtlException : CakeEventConversionsFailedEtlException
    {
        /// <summary>
        /// Gets or sets AffiliateId in the database for which statistics was extracted.
        /// </summary>
        public int? AffiliateId { get; set; }

        /// <inheritdoc cref="FailedEtlException"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="CakeAffSubSumsFailedEtlException"/> class.
        /// </summary>
        /// <param name="startDate">Start date from which statistics was extracted.</param>
        /// <param name="endDate">End date to which statistics was extracted.</param>
        /// <param name="affiliateId">AffiliateId in the database for which statistics was extracted.</param>
        /// <param name="advertiserId">AdvertiserId in the database for which statistics was extracted.</param>
        /// <param name="offerId">OfferId in the database for which statistics was extracted.</param>
        /// <param name="innerException">The source exception.</param>
        public CakeAffSubSumsFailedEtlException(
            DateTime? startDate,
            DateTime? endDate,
            int? affiliateId,
            int? advertiserId,
            int? offerId,
            Exception innerException)
            : base(startDate, endDate, advertiserId, offerId, innerException)
        {
            AffiliateId = affiliateId;
        }
    }
}