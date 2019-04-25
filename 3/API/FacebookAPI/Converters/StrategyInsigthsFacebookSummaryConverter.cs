using FacebookAPI.Entities;

namespace FacebookAPI.Converters
{
    /// <summary>
    /// Strategy insights summary converter.
    /// </summary>
    /// <seealso cref="FacebookAPI.Converters.FacebookSummaryConverter" />
    internal class StrategyInsigthsFacebookSummaryConverter : FacebookSummaryConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyInsigthsFacebookSummaryConverter"/> class.
        /// </summary>
        /// <param name="conversionActionType">Type of the conversion action.</param>
        /// <param name="clickAttribution">The click attribution.</param>
        /// <param name="viewAttribution">The view attribution.</param>
        public StrategyInsigthsFacebookSummaryConverter(string conversionActionType,
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
            var summary = GetFacebookSummaryMetricsFromRow(row);
            ProcessConversionValuesActions(row, summary);
            return summary;
        }
    }
}
