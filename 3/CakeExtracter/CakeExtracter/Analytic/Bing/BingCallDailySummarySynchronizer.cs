using CakeExtracter.Analytic.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        protected override string TargetAnalyticTable => "BingCallDailySummary";
    }
}