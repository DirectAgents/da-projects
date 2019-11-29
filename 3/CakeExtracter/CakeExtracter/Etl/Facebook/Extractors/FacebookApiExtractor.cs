using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI.Providers;

namespace CakeExtracter.Etl.Facebook.Extractors
{
    /// <summary>
    /// BaseFacebook Api extractor.
    /// </summary>
    /// <seealso cref="Etl.Extracter{T}" />
    public abstract class FacebookApiExtractor<T, TProvider> : Extracter<T>
    where TProvider : FacebookInsightsDataProvider
    {
        protected readonly TProvider _fbUtility;
        protected readonly DateRange? dateRange;
        protected readonly int accountId;   // in our db
        protected readonly string fbAccountId; // fb account: aka "ad account"

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiExtractor{T, TProvider}"/> class.
        /// </summary>
        /// <param name="fbUtility">The fb utility.</param>
        /// <param name="dateRange">The date range.</param>
        /// <param name="account">The account.</param>
        protected FacebookApiExtractor(TProvider fbUtility, DateRange? dateRange, ExtAccount account)
        {
            this._fbUtility = fbUtility;
            this.dateRange = dateRange;
            this.accountId = account.Id;
            this.fbAccountId = account.ExternalId;
        }
    }
}
