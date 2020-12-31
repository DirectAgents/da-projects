using System;
using CakeExtracter.Analytic.Common;

namespace CakeExtracter.Analytic.Adform
{
    /// <summary>
    /// Synchronizer for Adform Tracking Point level.
    /// </summary>
    internal class AdformTrackingPointSummarySynchronizer : BaseAnalyticSynchronizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdformTrackingPointSummarySynchronizer"/> class.
        /// </summary>
        /// <param name="startDate">Start date to synch the analytic data.</param>
        /// <param name="endDate">End date to synch the analytic data.</param>
        /// <param name="accountId">Account identifier to synch the analytic data.</param>
        public AdformTrackingPointSummarySynchronizer(DateTime startDate, DateTime endDate, int accountId)
            : base(startDate, endDate, accountId)
        {
        }

        /// <inheritdoc/>
        public override string TargetAnalyticTable => "AdformTrackingPointSummary";
    }
}
