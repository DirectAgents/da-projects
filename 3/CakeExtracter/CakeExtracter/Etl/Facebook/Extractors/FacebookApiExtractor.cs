using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI.Providers;

namespace CakeExtracter.Etl.Facebook.Extractors
{
    /// <summary>
    /// BaseFacebook Api extractor.
    /// </summary>
    /// <typeparam name="TSummary">Summary entity.</typeparam>
    /// <typeparam name="TDataProvider">Data provider.</typeparam>
    public abstract class FacebookApiExtractor<TSummary, TDataProvider> : Extracter<TSummary>
        where TDataProvider : FacebookInsightsDataProvider
    {
        protected readonly TDataProvider FbUtility;
        protected readonly DateRange? DateRange;
        protected readonly int AccountId; // in our db
        protected readonly string FbAccountId; // fb account: aka "ad account"

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookApiExtractor{TSummary, TDataProvider}"/> class.
        /// </summary>
        /// <param name="fbUtility">The fb utility.</param>
        /// <param name="dateRange">The date range.</param>
        /// <param name="account">The account.</param>
        protected FacebookApiExtractor(TDataProvider fbUtility, DateRange? dateRange, ExtAccount account)
        {
            FbUtility = fbUtility;
            DateRange = dateRange;
            AccountId = account.Id;
            FbAccountId = account.ExternalId;
        }
    }
}
