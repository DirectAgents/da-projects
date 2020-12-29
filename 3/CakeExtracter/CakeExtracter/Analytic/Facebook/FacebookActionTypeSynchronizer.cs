using System;
using CakeExtracter.Analytic.Common;

namespace CakeExtracter.Analytic.Facebook
{
    /// <summary>
    /// Synchronizer for Facebook Action Type Summary level.
    /// </summary>
    internal class FacebookActionTypeSynchronizer : BaseAnalyticSynchronizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookActionTypeSynchronizer"/> class.
        /// </summary>
        /// <param name="startDate">Start date to synch the analytic data.</param>
        /// <param name="endDate">End date to synch the analytic data.</param>
        /// <param name="accountId">Account identifier to synch the analytic data.</param>
        public FacebookActionTypeSynchronizer(DateTime startDate, DateTime endDate, int accountId)
            : base(startDate, endDate, accountId)
        {
        }

        /// <inheritdoc/>
        public override string TargetAnalyticTable => "FacebookActionTypeSummary";
    }
}