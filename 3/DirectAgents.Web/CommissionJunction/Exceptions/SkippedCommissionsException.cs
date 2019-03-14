using System;

namespace CommissionJunction.Exceptions
{
    internal class SkippedCommissionsException :Exception
    {
        public SkippedCommissionsException(string sinceTime, string beforeTime, string accountId, Exception exception) : base(
            $"Commissions were not extracted from the API for the following arguments: accountExternalId = {accountId}, sinceTime = {sinceTime}, beforeTime = {beforeTime}, exception - {exception.Message}")
        {
        }
    }
}
