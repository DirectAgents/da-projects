using OfficeOpenXml;

namespace Spreadsheets
{
    public class TestSpreadSheet : SpreadsheetBase
    {
        public TestSpreadSheet()
        {
            this.ExcelPackage = new ExcelPackage();
            this.ExcelPackage.Workbook.Properties.Title = "Test Spreadsheet";

            this.ExcelPackage.Workbook.Worksheets.Add("sample worksheet");
            var ws = this.ExcelPackage.Workbook.Worksheets[1];
            ws.Cells.Style.Font.Size = 11;
            ws.Cells.Style.Font.Name = "Calibri";

            ws.Cells[1, 1].Value = "testing";
        }
    }
}
