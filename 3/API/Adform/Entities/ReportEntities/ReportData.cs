using System;
using System.Collections.Generic;
using Adform.Enums;
using Adform.Helpers;

namespace Adform.Entities.ReportEntities
{
    /// <summary>
    /// The class represents returned report data.
    /// </summary>
    public class ReportData
    {
        //These constants must match column IDs from the column Lookup.
        public readonly string AdInteractionColumnId = AdformApiHelper.Dimensions[Dimension.AdInteractionType];
        public readonly string OrderColumnId = AdformApiHelper.Dimensions[Dimension.Order];
        public readonly string CampaignColumnId = AdformApiHelper.Dimensions[Dimension.Campaign];
        public readonly string DateColumnId = AdformApiHelper.Dimensions[Dimension.Date];
        public readonly string LineItemColumnId = AdformApiHelper.Dimensions[Dimension.LineItem];
        public readonly string BannerColumnId = AdformApiHelper.Dimensions[Dimension.Banner];
        public readonly string MediaColumnId = AdformApiHelper.Dimensions[Dimension.Media];
        public readonly string ImpressionColumnId = AdformApiHelper.ImpressionsMetric;
        public readonly string ClickColumnId = AdformApiHelper.ClicksMetric;
        public readonly string CostColumnId = AdformApiHelper.CostMetric;
        public readonly string ConversionAllColumnId = GetColumnId(AdformApiHelper.ConversionsMetric, AdformApiHelper.ConversionTypeAll);
        public readonly string Conversion1ColumnId = GetColumnId(AdformApiHelper.ConversionsMetric, AdformApiHelper.ConversionType1);
        public readonly string Conversion2ColumnId = GetColumnId(AdformApiHelper.ConversionsMetric, AdformApiHelper.ConversionType2);
        public readonly string Conversion3ColumnId = GetColumnId(AdformApiHelper.ConversionsMetric, AdformApiHelper.ConversionType3);
        public readonly string SalesAllColumnId = GetColumnId(AdformApiHelper.SalesMetric, AdformApiHelper.ConversionTypeAll);
        public readonly string Sales1ColumnId = GetColumnId(AdformApiHelper.SalesMetric, AdformApiHelper.ConversionType1);
        public readonly string Sales2ColumnId = GetColumnId(AdformApiHelper.SalesMetric, AdformApiHelper.ConversionType2);
        public readonly string Sales3ColumnId = GetColumnId(AdformApiHelper.SalesMetric, AdformApiHelper.ConversionType3);

        /// <summary>
        /// Lists dimension keys (names) followed by the metric keys (names) with their specs. 
        /// </summary>
        public List<Dictionary<string, dynamic>> columns { get; set; }

        /// <summary>
        /// Data rows, where each row contains a list of dimension values followed by the metric values.
        /// </summary>
        public List<List<object>> rows { get; set; }
        // totals?

        /// <summary>
        /// The method returns a dictionary that matches a unique name of a column with its number.
        /// </summary>
        public Dictionary<string, int> CreateColumnLookup()
        {
            var columnLookup = new Dictionary<string, int>();
            for (var i = 0; i < columns.Count; i++)
            {
                var columnId = GetColumnId(columns[i]);
                columnLookup[columnId] = i;
            }

            return columnLookup;
        }

        private string GetColumnId(Dictionary<string, dynamic> columnMetadata)
        {
            var conversionType = columnMetadata.ContainsKey("specs") && columnMetadata["specs"].ContainsKey(AdformApiHelper.SpecsConversionType)
                ? columnMetadata["specs"][AdformApiHelper.SpecsConversionType]
                : string.Empty;
            var columnId = GetColumnId(columnMetadata["key"], conversionType);
            return columnId;
        }

        private static string GetColumnId(string columnName, string conversionType)
        {
            return string.IsNullOrEmpty(conversionType) ? columnName : $"{columnName} - {conversionType}";
        }
    }
}