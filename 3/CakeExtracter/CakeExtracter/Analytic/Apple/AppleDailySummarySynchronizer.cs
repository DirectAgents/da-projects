using System;
using CakeExtracter.Analytic.Common;

namespace CakeExtracter.Analytic.Apple
{
    /// <summary>
    /// Synchronizer for Apple Daily Summary level.
    /// </summary>
    internal class AppleDailySummarySynchronizer : BaseAnalyticSynchronizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppleDailySummarySynchronizer"/> class.
        /// </summary>
        /// <param name="startDate">Start date to synch the analytic data.</param>
        /// <param name="endDate">End date to synch the analytic data.</param>
        public AppleDailySummarySynchronizer(DateTime startDate, DateTime endDate)
            : base(startDate, endDate)
        {
        }

        /// <inheritdoc/>
        protected override string TargetAnalyticTable => "AppleDailySummary";
    }
}
