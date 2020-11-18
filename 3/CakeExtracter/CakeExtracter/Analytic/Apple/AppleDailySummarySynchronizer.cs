using CakeExtracter.Analytic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Synchronizer for Apple Daily Summary level.
/// </summary>
namespace CakeExtracter.Analytic.Apple
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppleDailySummarySynchronizer"/> class.
    /// </summary>
    /// <param name="startDate">Start date to synch the analytic data.</param>
    /// <param name="endDate">End date to synch the analytic data.</param>
    internal class AppleDailySummarySynchronizer : BaseAnalyticSynchronizer
    {
        public AppleDailySummarySynchronizer(DateTime startDate, DateTime endDate)
            : base(startDate, endDate)
        {
        }

        /// <inheritdoc/>
        protected override string TargetAnalyticTable => "AppleDailySummary";
    }
}
