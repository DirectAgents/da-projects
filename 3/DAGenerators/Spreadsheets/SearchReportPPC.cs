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
        private const int StartRow_Monthly = 13;
        private const int Row_SummaryDate = 8;
        private const int Row_WeeklyHeader = 11;
        private const int Row_ClientNameBottom = 14;
        private const int Row_WeeklyChart = 15;

        private const int Col_StatsTitle = 2;
        private const int Col_Clicks = 3;
        private const int Col_Impressions = 4;
        private const int Col_Orders = 5;
        private const int Col_Cost = 7;
        private const int Col_Revenue = 8;

        private const int Col_OrderRate = 6;
        private const int Col_Net = 9;
        private const int Col_RevPerOrder = 10;
        private const int Col_CTR = 11;
        private const int Col_CPC = 12;
        private const int Col_CPO = 13;
        private const int Col_ROAS = 14;
        private const int Col_ROI = 15;

        ExcelWorksheet WS { get { return this.ExcelPackage.Workbook.Worksheets[1]; } }
        int NumWeeksAdded { get; set; }
        int NumMonthsAdded { get; set; }

        public SearchReportPPC(string templateFolder)
        {
            var fileInfo = new FileInfo(Path.Combine(templateFolder, TemplateFilename));
            this.ExcelPackage = new ExcelPackage(fileInfo);

            SetReportDate(DateTime.Today);
        }

        public void SetReportDate(DateTime date)
        {
            WS.Cells[Row_SummaryDate, 2].Value = "Report Summary " + date.ToShortDateString();
        }

        // Load monthly stats first, so we know where to put the weekly chart and the client name text.
        public void LoadMonthlyStats<T>(IEnumerable<T> stats, IList<string> propertyNames)
        {
            LoadWeeklyMonthlyStats(stats, propertyNames, StartRow_Monthly);
            NumMonthsAdded = stats.Count();
        }
        public void LoadWeeklyStats<T>(IEnumerable<T> stats, IList<string> propertyNames)
        {
            LoadWeeklyMonthlyStats(stats, propertyNames, StartRow_Weekly);
            NumWeeksAdded = stats.Count();
            CreateWeeklyChart_RevenueVsClicks();
        }
        public void SetClientName(string clientName)
        {
            WS.Cells[Row_ClientNameBottom + NumWeeksAdded + NumMonthsAdded, 2].Value = "Direct Agents | " + clientName;
        }

        public void CreateWeeklyChart_RevenueVsClicks()
        {
            CreateWeeklyChart(NumWeeksAdded, Col_Revenue, "Revenue", Col_Clicks, "Clicks");
        }
        private void CreateWeeklyChart(int numWeeks, int series1column, string series1name, int series2column, string series2name)
        {
            var chart = WS.Drawings.AddChart("chartWeekly", eChartType.ColumnClustered);
            chart.SetPosition(Row_WeeklyChart + NumWeeksAdded + NumMonthsAdded, 0, 1, 0);
            chart.SetSize(1071, 217);

            chart.Title.Text = "Weekly Performance"; // TODO: add year; remember: handle when it's two years
            chart.Title.Font.Bold = true;
            //chart.Title.Anchor = eTextAnchoringType.Bottom;
            //chart.Title.AnchorCtr = false;

            var series = chart.Series.Add(new ExcelAddress(StartRow_Weekly, series1column, StartRow_Weekly + numWeeks - 1, series1column).Address,
                                          new ExcelAddress(StartRow_Weekly, Col_StatsTitle, StartRow_Weekly + numWeeks - 1, Col_StatsTitle).Address);
            //series.HeaderAddress = new ExcelAddress(Row_WeeklyHeader, column1, Row_WeeklyHeader, column1);
            series.Header = series1name;

            var chartType2 = chart.PlotArea.ChartTypes.Add(eChartType.LineMarkers);
            chartType2.UseSecondaryAxis = true;
            chartType2.XAxis.Deleted = true;
            var series2 = chartType2.Series.Add(new ExcelAddress(StartRow_Weekly, series2column, StartRow_Weekly + numWeeks - 1, series2column).Address,
                                                new ExcelAddress(StartRow_Weekly, Col_StatsTitle, StartRow_Weekly + numWeeks - 1, Col_StatsTitle).Address);
            //series2.HeaderAddress = new ExcelAddress(Row_WeeklyHeader, column2, Row_WeeklyHeader, column2);
            series2.Header = series2name;
        }

        // propertyNames for: title, clicks, impressions, orders, cost, revenue
        private void LoadWeeklyMonthlyStats<T>(IEnumerable<T> stats, IList<string> propertyNames, int startingRow)
        {
            int numRows = stats.Count();
            if (numRows > 0)
            {
                WS.InsertRow(startingRow, numRows, startingRow); // # rows inserted == size of the stats enumerable

                LoadColumnFromStats(stats, startingRow, Col_StatsTitle, propertyNames[0]);
                LoadColumnFromStats(stats, startingRow, Col_Clicks, propertyNames[1]);
                LoadColumnFromStats(stats, startingRow, Col_Impressions, propertyNames[2]);
                LoadColumnFromStats(stats, startingRow, Col_Orders, propertyNames[3]);
                LoadColumnFromStats(stats, startingRow, Col_Cost, propertyNames[4]);
                LoadColumnFromStats(stats, startingRow, Col_Revenue, propertyNames[5]);

                for (int i = 0; i < numRows; i++)
                {
                    LoadStatsRowFormulas(startingRow + i);
                }
            }
        }

        private void LoadColumnFromStats<T>(IEnumerable<T> stats, int startingRow, int iColumn, string propertyName)
        {
            var type = stats.First().GetType();
            WS.Cells[startingRow, iColumn].LoadFromCollection(stats, false, TableStyles.None, BindingFlags.Default, new[] { type.GetProperty(propertyName) });
        }

        private void LoadStatsRowFormulas(int iRow)
        {
            WS.Cells[iRow, Col_OrderRate].FormulaR1C1 = String.Format("RC[{0}]/RC[{1}]", Col_Orders - Col_OrderRate, Col_Clicks - Col_OrderRate); // OrderRate (Orders/Clicks)
            WS.Cells[iRow, Col_Net].FormulaR1C1 = String.Format("RC[{0}]-RC[{1}]", Col_Revenue - Col_Net, Col_Cost - Col_Net); // Net (Rev-Cost)
            WS.Cells[iRow, Col_RevPerOrder].FormulaR1C1 = String.Format("RC[{0}]/RC[{1}]", Col_Revenue - Col_RevPerOrder, Col_Orders - Col_RevPerOrder); // Revenue/Orders
            WS.Cells[iRow, Col_CTR].FormulaR1C1 = String.Format("RC[{0}]/RC[{1}]", Col_Clicks - Col_CTR, Col_Impressions - Col_CTR); // CTR (Clicks/Impressions)
            WS.Cells[iRow, Col_CPC].FormulaR1C1 = String.Format("RC[{0}]/RC[{1}]", Col_Cost - Col_CPC, Col_Clicks - Col_CPC); // CPC (Cost/Clicks)
            WS.Cells[iRow, Col_CPO].FormulaR1C1 = String.Format("RC[{0}]/RC[{1}]", Col_Cost - Col_CPO, Col_Orders - Col_CPO); // CPO (Cost/Orders)
            WS.Cells[iRow, Col_ROAS].FormulaR1C1 = String.Format("RC[{0}]/RC[{1}]", Col_Revenue - Col_ROAS, Col_Cost - Col_ROAS); // ROAS (Rev/Cost)
            WS.Cells[iRow, Col_ROI].FormulaR1C1 = String.Format("(RC[{0}]-RC[{1}])/RC[{1}]", Col_Revenue - Col_ROI, Col_Cost - Col_ROI); // ROI ((Rev-Cost)/Cost)
        }
    }
}
