﻿using FacebookAPI.Entities;

namespace FacebookAPI.Converters
{
    internal class DailyInsigthsFacebookSummaryConverter : FacebookSummaryConverter
    {
        string conversionActionType;
        string clickAttribution;
        string viewAttribution;

        public DailyInsigthsFacebookSummaryConverter(string conversionActionType,
            string clickAttribution, string viewAttribution) : base(conversionActionType, clickAttribution, viewAttribution)
        {
            this.conversionActionType = conversionActionType;
            this.clickAttribution = clickAttribution;
            this.viewAttribution = viewAttribution;
        }

        public override FBSummary ParseSummaryRow(dynamic row)
        {
            var summary = GetFacebokSummaryMetricsFromRow(row);
            ProcessConversionValuesActions(row, summary);
            return summary;
        }
    }
}
