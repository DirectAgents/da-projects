using System;
using CakeExtracter.Analytic.Common;

namespace CakeExtracter.Analytic.Bing
{
    /// <summary>
    /// Synchronizer for AdWords Daily Summary level.
    /// </summary>
    internal class BingDailySummarySynchronizer : BaseAnalyticSynchronizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BingDailySummarySynchronizer"/> class.
        /// </summary>
        /// <param name="startDate">Start date to synch the analytic data.</param>
        /// <param name="endDate">End date to synch the analytic data.</param>
        public BingDailySummarySynchronizer(DateTime startDate, DateTime endDate)
            : base(startDate, endDate)
        {
        }

        /// <inheritdoc/>
        protected override string TargetAnalyticTable => "BingDailySummary";
    }
}