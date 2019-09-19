using System.Globalization;
using System.Text;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading.Constants;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading
{
    public static class VcdComposingOfReportHelper
    {
        public static dynamic GetReportProductsRows(dynamic reportData)
        {
            var firstReportPart = GetFirstReportPart(reportData);
            return firstReportPart[ResponseDataConstants.ReportRowsNode];
        }

        public static StringBuilder GetLineOfReportColumns(dynamic reportData)
        {
            var columnNamesData = GetColumnNamesData(reportData);
            var columnLine = ComposeColumnLineFromDynamicData(columnNamesData);
            RemoveFirstSymbolFromLine(columnLine);
            RemoveLastSymbolFromLine(columnLine);
            return columnLine;
        }

        public static StringBuilder CreateRowLineOfReport(dynamic productData)
        {
            var productValuesData = productData[ResponseDataConstants.RecordValuesNode];
            var rowLine = ComposeRowLineFromDynamicData(productValuesData);
            RemoveLastSymbolFromLine(rowLine);
            return rowLine;
        }

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
                var dataValue = $"\"{productValuesData[i][ResponseDataConstants.ValueNode]}\"{ResponseDataConstants.CorrectValueDelimiter}";
                rowLine.Append(dataValue);
            }
            return rowLine;
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
            line.Remove(line.Length - 1, 1);
        }
    }
}
