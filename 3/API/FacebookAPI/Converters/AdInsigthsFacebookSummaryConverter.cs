using FacebookAPI.Entities;

namespace FacebookAPI.Converters
{
    /// <summary>
    /// Ad Level Summary converted
    /// </summary>
    /// <seealso cref="FacebookAPI.Converters.FacebookSummaryConverter" />
    internal class AdInsigthsFacebookSummaryConverter : FacebookSummaryConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdInsigthsFacebookSummaryConverter"/> class.
        /// </summary>
        /// <param name="conversionActionType">Type of the conversion action.</param>
        /// <param name="clickAttribution">The click attribution.</param>
        /// <param name="viewAttribution">The view attribution.</param>
        public AdInsigthsFacebookSummaryConverter(string conversionActionType,
            string clickAttribution, string viewAttribution) : base(conversionActionType, clickAttribution, viewAttribution)
        {
        }

        /// <summary>
        /// Parses the summary row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        public override FBSummary ParseSummaryRow(dynamic row)
        {
            var summary = GetFacebokSummaryMetricsFromRow(row);
            ProcessAllActions(row, summary);
            return summary;
        }
    }
}
