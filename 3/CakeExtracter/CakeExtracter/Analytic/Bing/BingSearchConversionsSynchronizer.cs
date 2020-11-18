using System;
using CakeExtracter.Analytic.Common;

namespace CakeExtracter.Analytic.Bing
{
    /// <summary>
    /// Synchronizer for Bing Search Conversion Summary level.
    /// </summary>
    internal class BingSearchConversionsSynchronizer : BaseAnalyticSynchronizer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BingSearchConversionsSynchronizer"/> class.
        /// </summary>
        /// <param name="startDate">Start date to synch the analytic data.</param>
        /// <param name="endDate">End date to synch the analytic data.</param>
        public BingSearchConversionsSynchronizer(DateTime startDate, DateTime endDate)
            : base(startDate, endDate)
        {
        }

        /// <inheritdoc/>
        protected override string TargetAnalyticTable => "BingSearchConverions";
    }
}
