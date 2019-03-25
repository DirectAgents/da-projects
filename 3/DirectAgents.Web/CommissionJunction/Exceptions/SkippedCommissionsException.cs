using System;

namespace CommissionJunction.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Exception for failed commission requests for a specific date range and account.
    /// </summary>
    internal class SkippedCommissionsException :Exception
    {
        private const string ExceptionMessage = "Commissions were not extracted from the API for the following arguments: ";

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="SkippedCommissionsException"/> class.
        /// </summary>
        /// <param name="sinceTime">Commissions dates are greater than or equal this param.</param>
        /// <param name="beforeTime">Commissions dates are less than this param.</param>
        /// <param name="accountId">Account id</param>
        /// <param name="exception">Source exception</param>
        public SkippedCommissionsException(string sinceTime, string beforeTime, string accountId, Exception exception) : base(
             ExceptionMessage + $"accountExternalId = {accountId}, sinceTime = {sinceTime}, beforeTime = {beforeTime}, exception - {exception.Message}")
        {
        }
    }
}
