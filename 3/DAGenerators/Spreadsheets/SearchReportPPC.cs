using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DAGenerators.Spreadsheets
{
    public class SearchReportPPC : SpreadsheetBase
    {
        ExcelWorksheet WS { get { return this.ExcelPackage.Workbook.Worksheets[1]; } }

        public SearchReportPPC(string templatePath)
        {
            var fileInfo = new FileInfo(templatePath);
            this.ExcelPackage = new ExcelPackage(fileInfo);

            SetReportDate(DateTime.Today);
        }

        public void SetReportDate(DateTime date)
        {
            WS.Cells[8, 2].Value = "Weekly Summary " + date.ToShortDateString();
        }

        public void LoadWeeklyStats<T>(IEnumerable<T> stats, IList<string> propertyNames)
        {
            LoadWeeklyMonthlyStats(stats, propertyNames, 12);
        }

        public void LoadMonthlyStats<T>(IEnumerable<T> stats, IList<string> propertyNames)
        {
            LoadWeeklyMonthlyStats(stats, propertyNames, 27);
        }

        // propertyNames for: title, clicks, impressions, orders, cost, revenue
        private void LoadWeeklyMonthlyStats<T>(IEnumerable<T> stats, IList<string> propertyNames, int startingRow)
        {
            int numRows = stats.Count();
            if (numRows > 0)
            {
                var type = stats.First().GetType();
                var members1 = propertyNames.Take(4).Select(p => type.GetProperty(p)).ToArray();
                var members2 = propertyNames.Skip(4).Select(p => type.GetProperty(p)).ToArray();

                WS.Cells[startingRow, 2].LoadFromCollection(stats, false, TableStyles.None, BindingFlags.Default, members1);
                WS.Cells[startingRow, 7].LoadFromCollection(stats, false, TableStyles.None, BindingFlags.Default, members2);

                for (int i = 0; i < numRows; i++)
                {
                    LoadStatsRowFormulas(startingRow + i);
                }
            }
        }

        private void LoadStatsRowFormulas(int iRow)
        {
            WS.Cells[iRow, 6].Formula = "E" + iRow + "/C" + iRow; // Order Rate
            WS.Cells[iRow, 9].Formula = "H" + iRow + "-G" + iRow; // Net
            WS.Cells[iRow, 10].Formula = "H" + iRow + "/E" + iRow; // Revenue/Order
            WS.Cells[iRow, 11].Formula = "C" + iRow + "/D" + iRow; // CTR
            WS.Cells[iRow, 12].Formula = "G" + iRow + "/C" + iRow; // CPC
            WS.Cells[iRow, 13].Formula = "H" + iRow + "/G" + iRow; // ROAS
            WS.Cells[iRow, 14].Formula = "(H" + iRow + "-G" + iRow + ")/G" + iRow; // ROI
        }
    }
}
