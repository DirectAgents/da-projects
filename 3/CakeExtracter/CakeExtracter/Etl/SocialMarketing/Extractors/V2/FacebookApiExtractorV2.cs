using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI;

namespace CakeExtracter.Etl.SocialMarketing.Extractors.V2
{
    /// <summary>
    /// BaseFacebook Api extractor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="CakeExtracter.Etl.Extracter{T}" />
    public abstract class FacebookApiExtractorV2<T> : Extracter<T>
    {
        protected readonly FacebookInsightsDataProvider _fbUtility;
        protected readonly DateRange? dateRange;
        protected readonly int accountId;   // in our db
        protected readonly string fbAccountId; // fb account: aka "ad account"

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiExtractorV2{T}"/> class.
        /// </summary>
        /// <param name="fbUtility">The fb utility.</param>
        /// <param name="dateRange">The date range.</param>
        /// <param name="account">The account.</param>
        public FacebookApiExtractorV2(FacebookInsightsDataProvider fbUtility, DateRange? dateRange, ExtAccount account)
        {
            this._fbUtility = fbUtility;
            this.dateRange = dateRange;
            this.accountId = account.Id;
            this.fbAccountId = account.ExternalId;
        }
    }
}
