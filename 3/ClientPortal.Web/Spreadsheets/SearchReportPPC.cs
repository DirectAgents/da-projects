using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace Spreadsheets
{
    public class SearchReportPPC : SpreadsheetBase
    {
        public SearchReportPPC(string templatePath)
        {
            var fileInfo = new FileInfo(templatePath);
            this.ExcelPackage = new ExcelPackage(fileInfo);

            var ws = this.ExcelPackage.Workbook.Worksheets[1];
            ws.Cells[8, 2].Value = "Weekly Summary a/b/c/d/e";
        }
    }
}
