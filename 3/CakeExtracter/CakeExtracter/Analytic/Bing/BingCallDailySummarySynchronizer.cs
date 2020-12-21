using System;
using CakeExtracter.Analytic.Common;

namespace CakeExtracter.Analytic.Bing
{
    /// <summary>
    /// Synchronizer for Bing Call Daily Summary level.
    /// </summary>
    internal class BingCallDailySummarySynchronizer : BaseAnalyticSynchronizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BingCallDailySummarySynchronizer"/> class.
        /// </summary>
        /// <param name="startDate">Start date to synch the analytic data.</param>
        /// <param name="endDate">End date to synch the analytic data.</param>
        public BingCallDailySummarySynchronizer(DateTime startDate, DateTime endDate)
            : base(startDate, endDate)
        {
        }

        /// <inheritdoc/>
        public override string TargetAnalyticTable => "BingCallDailySummary";
    }
}