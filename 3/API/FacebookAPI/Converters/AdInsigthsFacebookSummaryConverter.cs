using FacebookAPI.Entities;

namespace FacebookAPI.Converters
{
    internal class AdInsigthsFacebookSummaryConverter : FacebookSummaryConverter
    {
        public AdInsigthsFacebookSummaryConverter(string conversionActionType,
            string clickAttribution, string viewAttribution) : base(conversionActionType, clickAttribution, viewAttribution)
        {
        }

        public override FBSummary ParseSummaryRow(dynamic row)
        {
            var summary = GetFacebokSummaryMetricsFromRow(row);
            ProcessAllActions(row, summary);
            return summary;
        }
    }
}
