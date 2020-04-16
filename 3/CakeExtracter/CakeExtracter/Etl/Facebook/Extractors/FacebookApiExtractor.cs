using System;
using CakeExtracter.Common;
using CakeExtracter.Etl.Facebook.Exceptions;
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
        /// Action for exception of failed extraction.
        /// </summary>
        public event Action<FacebookFailedEtlException> ProcessFailedExtraction;

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

        protected virtual void OnProcessFailedExtraction(
            DateTime? startDate,
            DateTime? endDate,
            int? accountId,
            string statsType,
            Exception e)
        {
            Logger.Error(e);
            var exception = new FacebookFailedEtlException(startDate, endDate, accountId, statsType, e);
            ProcessFailedExtraction?.Invoke(exception);
        }
    }
}
