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
        public readonly string ImpressionColumnId = GetColumnId(AdformApiHelper.ImpressionsMetric, AdformApiHelper.SpecsValueAll);
        public readonly string ClickColumnId = GetColumnId(AdformApiHelper.ClicksMetric, AdformApiHelper.SpecsValueAll);
        public readonly string CostColumnId = AdformApiHelper.CostMetric;
        public readonly string ConversionAllColumnId = GetColumnId(AdformApiHelper.ConversionsMetric, AdformApiHelper.SpecsValueConversionTypeAll);
        public readonly string Conversion1ColumnId = GetColumnId(AdformApiHelper.ConversionsMetric, AdformApiHelper.SpecsValueConversionType1);
        public readonly string Conversion2ColumnId = GetColumnId(AdformApiHelper.ConversionsMetric, AdformApiHelper.SpecsValueConversionType2);
        public readonly string Conversion3ColumnId = GetColumnId(AdformApiHelper.ConversionsMetric, AdformApiHelper.SpecsValueConversionType3);
        public readonly string SalesAllColumnId = GetColumnId(AdformApiHelper.SalesMetric, AdformApiHelper.SpecsValueConversionTypeAll);
        public readonly string Sales1ColumnId = GetColumnId(AdformApiHelper.SalesMetric, AdformApiHelper.SpecsValueConversionType1);
        public readonly string Sales2ColumnId = GetColumnId(AdformApiHelper.SalesMetric, AdformApiHelper.SpecsValueConversionType2);
        public readonly string Sales3ColumnId = GetColumnId(AdformApiHelper.SalesMetric, AdformApiHelper.SpecsValueConversionType3);
        public readonly string UniqueImpressionsColumnId = GetColumnId(AdformApiHelper.ImpressionsMetric, AdformApiHelper.SpecsValueCampaignUnique);

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
        /// <returns>Dictionary that matches a unique name of a column with its number.</returns>
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

        private static string GetColumnId(IReadOnlyDictionary<string, dynamic> columnMetadata)
        {
            var specsValue = GetSpecsValue(columnMetadata);
            var columnId = GetColumnId(columnMetadata["key"], specsValue);
            return columnId;
        }

        private static string GetColumnId(string columnName, string specsValue)
        {
            return string.IsNullOrEmpty(specsValue) ? columnName : $"{columnName} - {specsValue}";
        }

        private static string GetSpecsValue(IReadOnlyDictionary<string, dynamic> columnMetadata)
        {
            var specsValue = string.Empty;
            if (!columnMetadata.ContainsKey("specs"))
            {
                return specsValue;
            }
            if (columnMetadata["specs"].ContainsKey(AdformApiHelper.SpecsConversionType))
            {
                specsValue = columnMetadata["specs"][AdformApiHelper.SpecsConversionType];
            }
            if (columnMetadata["specs"].ContainsKey(AdformApiHelper.SpecsAdUniqueness))
            {
                specsValue = columnMetadata["specs"][AdformApiHelper.SpecsAdUniqueness];
            }
            return specsValue;
        }
    }
}