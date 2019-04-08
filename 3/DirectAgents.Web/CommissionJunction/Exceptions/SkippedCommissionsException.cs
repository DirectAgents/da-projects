using System;

namespace CommissionJunction.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Exception for failed commission requests for a specific date range and account.
    /// </summary>
    public class SkippedCommissionsException :Exception
    {
        private const string ExceptionMessage = "Commissions were not extracted from the API for the following arguments: ";
        private const string DateFormat = "yyyy-MM-ddTHH:mm:ssZ";

        /// <summary>
        /// Account ID in Commission Junction portal for which statistics was extracted.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// Start date from which statistics was extracted.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date to which statistics was extracted.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="SkippedCommissionsException"/> class.
        /// </summary>
        /// <param name="sinceTime">Commissions dates are greater than or equal this param.</param>
        /// <param name="beforeTime">Commissions dates are less than this param.</param>
        /// <param name="accountId">Account id</param>
        /// <param name="exception">Source exception</param>
        public SkippedCommissionsException(DateTime sinceTime, DateTime beforeTime, string accountId, Exception exception) : base(
            ExceptionMessage + $"accountExternalId = {accountId}, sinceTime = {sinceTime.ToString(DateFormat)}, " +
            $"beforeTime = {beforeTime.ToString(DateFormat)}, exception - {exception.Message}")
        {
            StartDate = sinceTime;
            EndDate = beforeTime.AddDays(-1);
            AccountId = accountId;
        }
    }
}
