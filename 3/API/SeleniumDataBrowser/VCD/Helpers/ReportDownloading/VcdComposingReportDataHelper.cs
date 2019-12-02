using System.Globalization;
using System.Text;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Constants;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading
{
    /// <summary>
    /// Helper for composing of VCD report dynamic data to CSV reports.
    /// </summary>
    public static class VcdComposingReportDataHelper
    {
        /// <summary>
        /// Gets data of product rows.
        /// </summary>
        /// <param name="reportData">Report data node.</param>
        /// <returns>Product rows dynamic array.</returns>
        public static dynamic GetReportProductsRows(dynamic reportData)
        {
            var firstReportPart = GetFirstReportPart(reportData);
            return firstReportPart[ResponseDataConstants.ReportRowsNode];
        }

        /// <summary>
        /// Returns line with column names for report header.
        /// </summary>
        /// <param name="reportData">Report data node.</param>
        /// <returns>Line with column names for report header.</returns>
        public static StringBuilder CreateHeaderLineWithColumnNames(dynamic reportData)
        {
            var columnNamesData = GetColumnNamesData(reportData);
            var columnLine = ComposeColumnLineFromDynamicData(columnNamesData);
            RemoveFirstSymbolFromLine(columnLine);
            RemoveLastSymbolFromLine(columnLine);
            return columnLine;
        }

        /// <summary>
        /// Returns line with information about product for one report line.
        /// </summary>
        /// <param name="productData">Report data node.</param>
        /// <returns>Line with product information for one report line.</returns>
        public static StringBuilder CreateRowLineWithProductInfo(dynamic productData)
        {
            var productValuesData = productData[ResponseDataConstants.RecordValuesNode];
            var rowLine = ComposeRowLineFromDynamicData(productValuesData);
            RemoveLastDelimiterFromLine(rowLine);
            return rowLine;
        }

        /// <summary>
        /// Returns the total number of report rows that will be returned from the VCD backend.
        /// </summary>
        /// <param name="reportData">Report data node.</param>
        /// <returns>The total number of report rows.</returns>
        public static int GetTotalReportRowCount(dynamic reportData)
        {
            var reportPart = GetFirstReportPart(reportData);
            var rowCountDynamic = reportPart[ResponseDataConstants.RowCountNode];
            return int.Parse(rowCountDynamic, NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign);
        }

        private static StringBuilder ComposeColumnLineFromDynamicData(dynamic columnNamesData)
        {
            var columnNamesLine = new StringBuilder(columnNamesData.ToString());
            ChangeDefaultDelimiterToCorrectForLine(columnNamesLine);
            return columnNamesLine;
        }

        private static StringBuilder ComposeRowLineFromDynamicData(dynamic productValuesData)
        {
            var rowLine = new StringBuilder();
            for (var i = 0; i <= productValuesData.Count - 1; i++)
            {
                var productValue = productValuesData[i][ResponseDataConstants.ValueNode];
                AddProductValueInLine(productValue, rowLine);
            }
            return rowLine;
        }

        private static void AddProductValueInLine(dynamic productValue, StringBuilder rowLine)
        {
            var stringValueWithDelimiter = GetFormattedProductValue(productValue);
            rowLine.Append(stringValueWithDelimiter);
        }

        private static string GetFormattedProductValue(dynamic productValue)
        {
            var stringValue = productValue.ToString();
            var formattedStringValue = stringValue.Replace("\"", @"""""");
            return $"\"{formattedStringValue}\"{ResponseDataConstants.CorrectValueDelimiter}";
        }

        private static dynamic GetFirstReportPart(dynamic reportData)
        {
            return reportData[ResponseDataConstants.PayloadNode][ResponseDataConstants.ReportPartsNode][0];
        }

        private static dynamic GetColumnNamesData(dynamic reportData)
        {
            var firstReportPart = GetFirstReportPart(reportData);
            return firstReportPart[ResponseDataConstants.ColumnNamesNode];
        }

        private static void ChangeDefaultDelimiterToCorrectForLine(StringBuilder line)
        {
            line.Replace(ResponseDataConstants.DefaultValueDelimiter, ResponseDataConstants.CorrectValueDelimiter);
        }

        private static void RemoveFirstSymbolFromLine(StringBuilder line)
        {
            line.Remove(0, 1);
        }

        private static void RemoveLastSymbolFromLine(StringBuilder line)
        {
            var startIndexForRemoving = line.Length - 1;
            line.Remove(startIndexForRemoving, 1);
        }

        private static void RemoveLastDelimiterFromLine(StringBuilder line)
        {
            var lengthDelimiter = ResponseDataConstants.CorrectValueDelimiter.Length;
            var startIndexForRemoving = line.Length - lengthDelimiter;
            line.Remove(startIndexForRemoving, lengthDelimiter);
        }
    }
}