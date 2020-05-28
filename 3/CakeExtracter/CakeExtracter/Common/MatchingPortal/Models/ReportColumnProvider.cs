using System;

namespace CakeExtracter.Common.MatchingPortal.Models
{
    /// <summary>
    /// A provider to extract values from columns for report building.
    /// </summary>
    public class ReportColumnProvider
    {
        /// <summary>
        /// Gets or sets name of the provided column.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets value extractor function.
        /// </summary>
        public Func<MatchResult, string> ValueExtractor { get; set; }
    }
}
