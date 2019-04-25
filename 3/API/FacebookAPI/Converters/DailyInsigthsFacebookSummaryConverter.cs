﻿using FacebookAPI.Entities;

namespace FacebookAPI.Converters
{
    /// <summary>
    /// Daily insights summary converter.
    /// </summary>
    /// <seealso cref="FacebookAPI.Converters.FacebookSummaryConverter" />
    internal class DailyInsigthsFacebookSummaryConverter : FacebookSummaryConverter
    {
        string conversionActionType;
        string clickAttribution;
        string viewAttribution;

        /// <summary>
        /// Initializes a new instance of the <see cref="DailyInsigthsFacebookSummaryConverter"/> class.
        /// </summary>
        /// <param name="conversionActionType">Type of the conversion action.</param>
        /// <param name="clickAttribution">The click attribution.</param>
        /// <param name="viewAttribution">The view attribution.</param>
        public DailyInsigthsFacebookSummaryConverter(string conversionActionType,
            string clickAttribution, string viewAttribution) : base(conversionActionType, clickAttribution, viewAttribution)
        {
            this.conversionActionType = conversionActionType;
            this.clickAttribution = clickAttribution;
            this.viewAttribution = viewAttribution;
        }

        /// <summary>
        /// Parses the summary row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        public override FBSummary ParseSummaryRow(dynamic row)
        {
            var summary = GetFacebokSummaryMetricsFromRow(row);
            ProcessConversionValuesActions(row, summary);
            return summary;
        }
    }
}
