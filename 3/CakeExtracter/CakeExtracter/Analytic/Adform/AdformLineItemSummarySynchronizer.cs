using System;
using CakeExtracter.Analytic.Common;

namespace CakeExtracter.Analytic.Adform
{
    /// <summary>
    /// Synchronizer for Adform LineItem level.
    /// </summary>
    internal class AdformLineItemSummarySynchronizer : BaseAnalyticSynchronizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdformLineItemSummarySynchronizer"/> class.
        /// </summary>
        /// <param name="startDate">Start date to synch the analytic data.</param>
        /// <param name="endDate">End date to synch the analytic data.</param>
        /// <param name="accountId">Account identifier to synch analytic date.</param>
        public AdformLineItemSummarySynchronizer(DateTime startDate, DateTime endDate, int accountId)
            : base(startDate, endDate, accountId)
        {
        }

        /// <inheritdoc />
        public override string TargetAnalyticTable => "AdformLineItemSummary";
    }
}