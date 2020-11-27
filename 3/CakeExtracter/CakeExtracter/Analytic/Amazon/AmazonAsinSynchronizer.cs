using System;
using CakeExtracter.Analytic.Common;

namespace CakeExtracter.Analytic.Amazon
{
    /// <summary>
    /// Synchronizer for Amazon Asin Summary level.
    /// </summary>
    internal class AmazonAsinSynchronizer : BaseAnalyticSynchronizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonAsinSynchronizer"/> class.
        /// </summary>
        /// <param name="startDate">Start date to synch the analytic data.</param>
        /// <param name="endDate">End date to synch the analytic data.</param>
        /// <param name="accountId">Account identifier to synch the analytic data.</param>
        public AmazonAsinSynchronizer(DateTime startDate, DateTime endDate, int accountId)
            : base(startDate, endDate, accountId)
        {
        }

        /// <inheritdoc />
        protected override string TargetAnalyticTable => "AmazonAsinSummary";
    }
}