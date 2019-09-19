using System.Globalization;
using System.Text;

namespace SeleniumDataBrowser.VCD.Helpers.ReportDownloading
{
    public static class VcdComposingOfReportHelper
    {
        public static dynamic GetReportProductsRows(dynamic reportData)
        {
            var firstReportPart = GetFirstReportPart(reportData);
            return firstReportPart["reportRows"];
        }

        public static StringBuilder GetLineOfReportColumns(dynamic reportData)
        {
            var columnNamesData = GetColumnNamesData(reportData);
            var columnLine = ComposeColumnLineFromDynamicData(columnNamesData);
            return RemoveFirstSymbolFromLine(RemoveLastSymbolFromLine(columnLine));
        }

        public static StringBuilder CreateRowLineOfReport(dynamic productData)
        {
            var productValuesData = productData["recordValues"];
            var rowLine = ComposeRowLineFromDynamicData(productValuesData);
            return RemoveLastSymbolFromLine(rowLine);
        }

        public static int GetTotalReportRowCount(dynamic reportData)
        {
            var reportPart = GetFirstReportPart(reportData);
            return int.Parse(reportPart["rowCount"], NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign);
        }

        private static StringBuilder ComposeColumnLineFromDynamicData(dynamic columnNamesData)
        {
            var line = new StringBuilder();
            return line.Append(columnNamesData.ToString());
        }

        private static StringBuilder ComposeRowLineFromDynamicData(dynamic productValuesData)
        {
            const char valueSeparator = ',';
            var line = new StringBuilder();
            for (var i = 0; i <= productValuesData.Count - 1; i++)
            {
                var dataValue = $@"""{productValuesData[i]["val"]}""{valueSeparator}";
                line.Append(dataValue);
            }
            return line;
        }

        private static dynamic GetFirstReportPart(dynamic reportData)
        {
            return reportData["payload"]["reportParts"][0];
        }

        private static dynamic GetColumnNamesData(dynamic reportData)
        {
            var firstReportPart = GetFirstReportPart(reportData);
            return firstReportPart["columnNames"];
        }

        private static StringBuilder RemoveFirstSymbolFromLine(StringBuilder line)
        {
            return line.Remove(0, 1);
        }

        private static StringBuilder RemoveLastSymbolFromLine(StringBuilder line)
        {
            return line.Remove(line.Length - 1, 1);
        }
    }
}
