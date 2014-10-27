﻿using OfficeOpenXml;
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
    public class Metric
    {
        public Metric(int colNum, string displayName, bool show = true)
        {
            this.ColNum = colNum;
            this.DisplayName = displayName;
            this.Show = show;
        }

        public int ColNum { get; set; }
        public string DisplayName { get; set; }
        public bool Show { get; set; }
    }

    public class SearchReportPPC : SpreadsheetBase
    {
        private const string TemplateFilename = "SearchPPCtemplate.xlsx";
        private const int Row_SummaryDate = 8;
        private const int Row_StatsHeader = 11;
        private const int StartRow_Weekly = 12;
        private const int StartRow_Monthly = 13;
        private const int Row_ClientNameBottom = 14;
        private const int Row_WeeklyChart = 15;

        private const int Col_StatsTitle = 2;

        public Metric Metric_Clicks = new Metric(3, "Clicks");
        public Metric Metric_Impressions = new Metric(4, "Impressions");
        public Metric Metric_Orders = new Metric(5, "Orders");
        public Metric Metric_Cost = new Metric(7, "Cost");
        public Metric Metric_Revenue = new Metric(8, "Revenue");

        public Metric Metric_OrderRate = new Metric(6, "Order Rate");
        public Metric Metric_Net = new Metric(9, "Net");
        public Metric Metric_RevPerOrder = new Metric(10, "Revenue/Order");
        public Metric Metric_CTR = new Metric(11, "CTR");
        public Metric Metric_CPC = new Metric(12, "CPC");
        public Metric Metric_CPO = new Metric(13, "CPO");
        public Metric Metric_ROAS = new Metric(14, "ROAS");
        public Metric Metric_ROI = new Metric(15, "ROI");

        ExcelWorksheet WS { get { return this.ExcelPackage.Workbook.Worksheets[1]; } }
        int NumWeeksAdded { get; set; }
        int NumMonthsAdded { get; set; }

        public SearchReportPPC(string templateFolder)
        {
            var fileInfo = new FileInfo(Path.Combine(templateFolder, TemplateFilename));
            this.ExcelPackage = new ExcelPackage(fileInfo);

            SetReportDate(DateTime.Today);

            //SetColumnHeaders(); // do this after showing/hiding metrics
        }

        public void SetReportDate(DateTime date)
        {
            WS.Cells[Row_SummaryDate, 2].Value = "Report Summary " + date.ToShortDateString();
        }
        public void SetColumnHeaders()
        {
            var metrics = GetMetrics(true);
            foreach (var metric in metrics)
            {
                WS.Cells[Row_StatsHeader, metric.ColNum].Value = metric.DisplayName;
            }
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
            CreateWeeklyChart(NumWeeksAdded, Metric_Revenue.ColNum, Metric_Revenue.DisplayName, Metric_Clicks.ColNum, Metric_Clicks.DisplayName);
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
            //series.HeaderAddress = new ExcelAddress(Row_StatsHeader, column1, Row_StatsHeader, column1);
            series.Header = series1name;

            var chartType2 = chart.PlotArea.ChartTypes.Add(eChartType.LineMarkers);
            chartType2.UseSecondaryAxis = true;
            chartType2.XAxis.Deleted = true;
            var series2 = chartType2.Series.Add(new ExcelAddress(StartRow_Weekly, series2column, StartRow_Weekly + numWeeks - 1, series2column).Address,
                                                new ExcelAddress(StartRow_Weekly, Col_StatsTitle, StartRow_Weekly + numWeeks - 1, Col_StatsTitle).Address);
            //series2.HeaderAddress = new ExcelAddress(Row_StatsHeader, column2, Row_StatsHeader, column2);
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
                LoadColumnFromStats(stats, startingRow, Metric_Clicks.ColNum, propertyNames[1]);
                LoadColumnFromStats(stats, startingRow, Metric_Impressions.ColNum, propertyNames[2]);
                LoadColumnFromStats(stats, startingRow, Metric_Orders.ColNum, propertyNames[3]);
                LoadColumnFromStats(stats, startingRow, Metric_Cost.ColNum, propertyNames[4]);
                LoadColumnFromStats(stats, startingRow, Metric_Revenue.ColNum, propertyNames[5]);

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
            WS.Cells[iRow, Metric_OrderRate.ColNum].FormulaR1C1 = String.Format("RC[{0}]/RC[{1}]", Metric_Orders.ColNum - Metric_OrderRate.ColNum, Metric_Clicks.ColNum - Metric_OrderRate.ColNum); // OrderRate (Orders/Clicks)
            WS.Cells[iRow, Metric_Net.ColNum].FormulaR1C1 = String.Format("RC[{0}]-RC[{1}]", Metric_Revenue.ColNum - Metric_Net.ColNum, Metric_Cost.ColNum - Metric_Net.ColNum); // Net (Rev-Cost)
            WS.Cells[iRow, Metric_RevPerOrder.ColNum].FormulaR1C1 = String.Format("RC[{0}]/RC[{1}]", Metric_Revenue.ColNum - Metric_RevPerOrder.ColNum, Metric_Orders.ColNum - Metric_RevPerOrder.ColNum); // Revenue/Orders
            WS.Cells[iRow, Metric_CTR.ColNum].FormulaR1C1 = String.Format("RC[{0}]/RC[{1}]", Metric_Clicks.ColNum - Metric_CTR.ColNum, Metric_Impressions.ColNum - Metric_CTR.ColNum); // CTR (Clicks/Impressions)
            WS.Cells[iRow, Metric_CPC.ColNum].FormulaR1C1 = String.Format("RC[{0}]/RC[{1}]", Metric_Cost.ColNum - Metric_CPC.ColNum, Metric_Clicks.ColNum - Metric_CPC.ColNum); // CPC (Cost/Clicks)
            WS.Cells[iRow, Metric_CPO.ColNum].FormulaR1C1 = String.Format("RC[{0}]/RC[{1}]", Metric_Cost.ColNum - Metric_CPO.ColNum, Metric_Orders.ColNum - Metric_CPO.ColNum); // CPO (Cost/Orders)
            WS.Cells[iRow, Metric_ROAS.ColNum].FormulaR1C1 = String.Format("RC[{0}]/RC[{1}]", Metric_Revenue.ColNum - Metric_ROAS.ColNum, Metric_Cost.ColNum - Metric_ROAS.ColNum); // ROAS (Rev/Cost)
            WS.Cells[iRow, Metric_ROI.ColNum].FormulaR1C1 = String.Format("(RC[{0}]-RC[{1}])/RC[{1}]", Metric_Revenue.ColNum - Metric_ROI.ColNum, Metric_Cost.ColNum - Metric_ROI.ColNum); // ROI ((Rev-Cost)/Cost)
        }

        public IEnumerable<Metric> GetMetrics(bool shownOnly)
        {
            var metrics = new List<Metric>();
            PossiblyAddMetric(metrics, Metric_Clicks, shownOnly);
            PossiblyAddMetric(metrics, Metric_Impressions, shownOnly);
            PossiblyAddMetric(metrics, Metric_Orders, shownOnly);
            PossiblyAddMetric(metrics, Metric_Cost, shownOnly);
            PossiblyAddMetric(metrics, Metric_Revenue, shownOnly);
            PossiblyAddMetric(metrics, Metric_OrderRate, shownOnly);
            PossiblyAddMetric(metrics, Metric_Net, shownOnly);
            PossiblyAddMetric(metrics, Metric_RevPerOrder, shownOnly);
            PossiblyAddMetric(metrics, Metric_CTR, shownOnly);
            PossiblyAddMetric(metrics, Metric_CPC, shownOnly);
            PossiblyAddMetric(metrics, Metric_CPO, shownOnly);
            PossiblyAddMetric(metrics, Metric_ROAS, shownOnly);
            PossiblyAddMetric(metrics, Metric_ROI, shownOnly);

            return metrics;
        }
        public void PossiblyAddMetric(IList<Metric> metrics, Metric metric, bool onlyIfShown)
        {
            if (!onlyIfShown || metric.Show)
                metrics.Add(metric);
        }
    }
}
