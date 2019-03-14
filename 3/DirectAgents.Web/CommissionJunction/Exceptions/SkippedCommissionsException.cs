using System;

namespace CommissionJunction.Exceptions
{
    internal class SkippedCommissionsException :Exception
    {
        public SkippedCommissionsException(DateTime startTime, DateTime endTime, string accountId, Exception exception) : base(
            $"Commissions were not extracted from the API for the following arguments: accountExternalId = {accountId}, startTime = {startTime}, endTime = {endTime}, exception - {exception.Message}")
        {
        }
    }
}
