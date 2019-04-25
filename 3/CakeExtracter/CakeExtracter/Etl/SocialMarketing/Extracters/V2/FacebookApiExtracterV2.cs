using CakeExtracter.Common;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI;

namespace CakeExtracter.Etl.SocialMarketing.Extracters.V2
{
    public abstract class FacebookApiExtracterV2<T> : Extracter<T>
    {
        protected readonly FacebookInsightsDataProvider _fbUtility;
        protected readonly DateRange? dateRange;
        protected readonly int accountId;   // in our db
        protected readonly string fbAccountId; // fb account: aka "ad account"

        public FacebookApiExtracterV2(FacebookInsightsDataProvider fbUtility, DateRange? dateRange, ExtAccount account)
        {
            this._fbUtility = fbUtility;
            this.dateRange = dateRange;
            this.accountId = account.Id;
            this.fbAccountId = account.ExternalId;
        }
    }
}
