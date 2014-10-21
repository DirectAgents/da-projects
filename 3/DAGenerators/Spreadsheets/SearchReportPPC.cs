using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
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
        private const string TemplateFilename = "SearchPPCtemplate.xlsx";
        private const int StartRow_Weekly = 12;
        private const int StartRow_Monthly = 27;
        private const int Row_WeeklyHeader = 11;
        private const int Col_StatsTitle = 2;
        private const int Col_Clicks = 3;
        private const int Col_Revenue = 8;

        ExcelWorksheet WS { get { return this.ExcelPackage.Workbook.Worksheets[1]; } }

        public SearchReportPPC(string templateFolder, string clientName)
        {
            var fileInfo = new FileInfo(Path.Combine(templateFolder, TemplateFilename));
            this.ExcelPackage = new ExcelPackage(fileInfo);

            SetReportDate(DateTime.Today);
            SetClientName(clientName);
        }

        public void SetReportDate(DateTime date)
        {
            WS.Cells[8, 2].Value = "Weekly Summary " + date.ToShortDateString();
        }
        public void SetClientName(string clientName)
        {
            WS.Cells[45, 2].Value = "Direct Agents | " + clientName;
        }

        public void LoadWeeklyStats<T>(IEnumerable<T> stats, IList<string> propertyNames)
        {
            LoadWeeklyMonthlyStats(stats, propertyNames, StartRow_Weekly);
            CreateWeeklyChart(stats.Count());
        }
        public void CreateWeeklyChart(int numWeeks)
        {
            var chart = WS.Drawings.AddChart("chartWeekly", eChartType.ColumnClustered);
            chart.SetPosition(46, 0, 1, 0);
            chart.SetSize(1071, 217);

            chart.Title.Text = "Weekly Performance"; // TODO: add year; remember: handle when it's two years
            chart.Title.Font.Bold = true;
            //chart.Title.Anchor = eTextAnchoringType.Bottom;
            //chart.Title.AnchorCtr = false;

            var series = chart.Series.Add(new ExcelAddress(StartRow_Weekly, Col_Revenue, StartRow_Weekly + numWeeks - 1, Col_Revenue).Address,
                                          new ExcelAddress(StartRow_Weekly, Col_StatsTitle, StartRow_Weekly + numWeeks - 1, Col_StatsTitle).Address);
            //series.HeaderAddress = new ExcelAddress(Row_WeeklyHeader, Col_Revenue, Row_WeeklyHeader, Col_Revenue);
            series.Header = "Revenue";

            var chartType2 = chart.PlotArea.ChartTypes.Add(eChartType.LineMarkers);
            chartType2.UseSecondaryAxis = true;
            chartType2.XAxis.Deleted = true;
            var series2 = chartType2.Series.Add(new ExcelAddress(StartRow_Weekly, Col_Clicks, StartRow_Weekly + numWeeks - 1, Col_Clicks).Address,
                                                new ExcelAddress(StartRow_Weekly, Col_StatsTitle, StartRow_Weekly + numWeeks - 1, Col_StatsTitle).Address);
            //series2.HeaderAddress = new ExcelAddress(Row_WeeklyHeader, Col_Clicks, Row_WeeklyHeader, Col_Clicks);
            series2.Header = "Clicks";
        }

        public void LoadMonthlyStats<T>(IEnumerable<T> stats, IList<string> propertyNames)
        {
            LoadWeeklyMonthlyStats(stats, propertyNames, StartRow_Monthly);
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
