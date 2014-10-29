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
        protected int Row_SummaryDate = 8;
        protected int Row_StatsHeader = 11;
        private const int Row_ClientNameBottom = 14;
        private const int Row_WeeklyChart = 15;

        private const int Col_StatsTitle = 2;

        protected string TemplateFilename = "SearchPPCtemplate.xlsx";
        protected int StartRow_Weekly = 12;
        protected int StartRow_Monthly = 13;
        private bool WeeklyFirst
        {
            get { return StartRow_Weekly <= StartRow_Monthly; }
        }

        public Metric Metric_Clicks = new Metric(0, null, false, false);
        public Metric Metric_Impressions = new Metric(0, null, false, false);
        public Metric Metric_Orders = new Metric(0, null, false, false);
        public Metric Metric_Cost = new Metric(0, null, false, false);
        public Metric Metric_Revenue = new Metric(0, null, false, false);

        // Computed metrics
        public Metric Metric_OrderRate = new Metric(0, null, true, false);
        public Metric Metric_Net = new Metric(0, null, true, false);
        public Metric Metric_RevPerOrder = new Metric(0, null, true, false);
        public Metric Metric_CTR = new Metric(0, null, true, false);
        public Metric Metric_CPC = new Metric(0, null, true, false);
        public Metric Metric_CPO = new Metric(0, null, true, false);
        public Metric Metric_ROAS = new Metric(0, null, true, false);
        public Metric Metric_ROI = new Metric(0, null, true, false);

        protected ExcelWorksheet WS { get { return this.ExcelPackage.Workbook.Worksheets[1]; } }
        protected int NumWeeksAdded { get; set; }
        protected int NumMonthsAdded { get; set; }

        public void Setup(string templateFolder)
        {
            Setup();
            var fileInfo = new FileInfo(Path.Combine(templateFolder, TemplateFilename));
            this.ExcelPackage = new ExcelPackage(fileInfo);

            SetReportDate(DateTime.Today);
            SetColumnHeaders(); // do this after showing/hiding metrics
        }

        protected virtual void Setup()
        {
            Metric_Clicks = new Metric(3, "Clicks");
            Metric_Impressions = new Metric(4, "Impressions");
            Metric_Orders = new Metric(5, "Orders");
            Metric_Cost = new Metric(7, "Cost");
            Metric_Revenue = new Metric(8, "Revenue");

            Metric_OrderRate = new Metric(6, "Order Rate");
            Metric_Net = new Metric(9, "Net");
            Metric_RevPerOrder = new Metric(10, "Revenue/Order");
            Metric_CTR = new Metric(11, "CTR");
            Metric_CPC = new Metric(12, "CPC");
            Metric_CPO = new Metric(13, "CPO");
            Metric_ROAS = new Metric(14, "ROAS");
            Metric_ROI = new Metric(15, "ROI");
        }

        public virtual void SetReportDate(DateTime date)
        {
            WS.Cells[Row_SummaryDate, 2].Value = "Report Summary " + date.ToShortDateString();
        }
        public virtual void SetReportingPeriod(DateTime fromDate, DateTime toDate)
        {
        }

        public void SetColumnHeaders()
        {
            var metrics = GetMetrics(true);
            foreach (var metric in metrics)
            {
                WS.Cells[Row_StatsHeader, metric.ColNum].Value = metric.DisplayName;
            }
        }

        public virtual void SetClientName(string clientName)
        {
            WS.Cells[Row_ClientNameBottom + NumWeeksAdded + NumMonthsAdded, 2].Value = "Direct Agents | " + clientName;
        }

        public virtual void LoadWeeklyStats<T>(IEnumerable<T> stats, IList<string> propertyNames)
        {
            LoadWeeklyMonthlyStats(stats, propertyNames, StartRow_Weekly + (WeeklyFirst ? 0 : NumMonthsAdded));
            NumWeeksAdded += stats.Count();
            CreateWeeklyChart_RevenueVsClicks();
        }

        public void LoadMonthlyStats<T>(IEnumerable<T> stats, IList<string> propertyNames)
        {
            LoadWeeklyMonthlyStats(stats, propertyNames, StartRow_Monthly + (WeeklyFirst ? NumWeeksAdded : 0));
            NumMonthsAdded += stats.Count();
        }
        //public void LoadMonthlyCampaignStats<T>(IEnumerable<T> stats, IList<string> propertyNames)
        //{
        //    LoadWeeklyMonthlyStats(stats, propertyNames, StartRow_Monthly + nummon
        //}

        // propertyNames for: title, clicks, impressions, orders, cost, revenue
        protected void LoadWeeklyMonthlyStats<T>(IEnumerable<T> stats, IList<string> propertyNames, int startingRow, int blankRowsInTemplate = 0)
        {
            int numRows = stats.Count();
            if (numRows > 0)
            {
                int blankRowsToInsert = numRows - blankRowsInTemplate;
                if (blankRowsToInsert > 0)
                {
                    WS.InsertRowZ(startingRow + (blankRowsInTemplate > 1 ? 1 : 0), blankRowsToInsert, startingRow);
                }
                LoadColumnFromStats(stats, startingRow, Col_StatsTitle, propertyNames[0]);
                LoadColumnFromStats(stats, startingRow, Metric_Clicks.ColNum, propertyNames[1], Metric_Clicks);
                LoadColumnFromStats(stats, startingRow, Metric_Impressions.ColNum, propertyNames[2], Metric_Impressions);
                LoadColumnFromStats(stats, startingRow, Metric_Orders.ColNum, propertyNames[3], Metric_Orders);
                LoadColumnFromStats(stats, startingRow, Metric_Cost.ColNum, propertyNames[4], Metric_Cost);
                LoadColumnFromStats(stats, startingRow, Metric_Revenue.ColNum, propertyNames[5], Metric_Revenue);

                // if there are blank rows in the template, assume the formulas are there
                if (blankRowsInTemplate > 0)
                {
                    // if only one row was added, assume the formula is already in the blank row
                    if (numRows > 1)
                    {
                        var computedMetrics = GetComputedMetrics(true);
                        // TODO: copy from one cell to a range?
                        for (int iRow = startingRow + 1; iRow < startingRow + numRows; iRow++)
                        {
                            foreach (var metric in computedMetrics)
                                WS.Cells[startingRow, metric.ColNum].Copy(WS.Cells[iRow, metric.ColNum]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < numRows; i++)
                        LoadStatsRowFormulas(startingRow + i);
                }
            }
        }
        private void LoadColumnFromStats<T>(IEnumerable<T> stats, int startingRow, int iColumn, string propertyName, Metric metric = null)
        {
            if (metric == null || metric.Show)
            {
                var type = stats.First().GetType();
                WS.Cells[startingRow, iColumn].LoadFromCollection(stats, false, TableStyles.None, BindingFlags.Default, new[] { type.GetProperty(propertyName) });
            }
        }

        private void LoadStatsRowFormulas(int iRow)
        {
            // TODO: use IFERROR in formulas for div-by-0 checking
            CheckLoadStatsRowFormula(iRow, Metric_OrderRate, String.Format("RC[{0}]/RC[{1}]", Metric_Orders.ColNum - Metric_OrderRate.ColNum, Metric_Clicks.ColNum - Metric_OrderRate.ColNum)); // OrderRate (Orders/Clicks)
            CheckLoadStatsRowFormula(iRow, Metric_Net, String.Format("RC[{0}]-RC[{1}]", Metric_Revenue.ColNum - Metric_Net.ColNum, Metric_Cost.ColNum - Metric_Net.ColNum)); // Net (Rev-Cost)
            CheckLoadStatsRowFormula(iRow, Metric_RevPerOrder, String.Format("RC[{0}]/RC[{1}]", Metric_Revenue.ColNum - Metric_RevPerOrder.ColNum, Metric_Orders.ColNum - Metric_RevPerOrder.ColNum)); // Revenue/Orders
            CheckLoadStatsRowFormula(iRow, Metric_CTR, String.Format("RC[{0}]/RC[{1}]", Metric_Clicks.ColNum - Metric_CTR.ColNum, Metric_Impressions.ColNum - Metric_CTR.ColNum)); // CTR (Clicks/Impressions)
            CheckLoadStatsRowFormula(iRow, Metric_CPC, String.Format("RC[{0}]/RC[{1}]", Metric_Cost.ColNum - Metric_CPC.ColNum, Metric_Clicks.ColNum - Metric_CPC.ColNum)); // CPC (Cost/Clicks)
            CheckLoadStatsRowFormula(iRow, Metric_CPO, String.Format("RC[{0}]/RC[{1}]", Metric_Cost.ColNum - Metric_CPO.ColNum, Metric_Orders.ColNum - Metric_CPO.ColNum)); // CPO (Cost/Orders)
            CheckLoadStatsRowFormula(iRow, Metric_ROAS, String.Format("RC[{0}]/RC[{1}]", Metric_Revenue.ColNum - Metric_ROAS.ColNum, Metric_Cost.ColNum - Metric_ROAS.ColNum)); // ROAS (Rev/Cost)
            CheckLoadStatsRowFormula(iRow, Metric_ROI, String.Format("(RC[{0}]-RC[{1}])/RC[{1}]", Metric_Revenue.ColNum - Metric_ROI.ColNum, Metric_Cost.ColNum - Metric_ROI.ColNum)); // ROI ((Rev-Cost)/Cost)
        }
        private void CheckLoadStatsRowFormula(int iRow, Metric metric, string formula)
        {
            if (metric.Show)
                WS.Cells[iRow, metric.ColNum].FormulaR1C1 = formula;
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

            int startRow_Weekly = this.StartRow_Weekly + (WeeklyFirst ? 0 : NumMonthsAdded);

            var series = chart.Series.Add(new ExcelAddress(startRow_Weekly, series1column, startRow_Weekly + numWeeks - 1, series1column).Address,
                                          new ExcelAddress(startRow_Weekly, Col_StatsTitle, startRow_Weekly + numWeeks - 1, Col_StatsTitle).Address);
            //series.HeaderAddress = new ExcelAddress(Row_StatsHeader, column1, Row_StatsHeader, column1);
            series.Header = series1name;

            var chartType2 = chart.PlotArea.ChartTypes.Add(eChartType.LineMarkers);
            chartType2.UseSecondaryAxis = true;
            chartType2.XAxis.Deleted = true;
            var series2 = chartType2.Series.Add(new ExcelAddress(startRow_Weekly, series2column, startRow_Weekly + numWeeks - 1, series2column).Address,
                                                new ExcelAddress(startRow_Weekly, Col_StatsTitle, startRow_Weekly + numWeeks - 1, Col_StatsTitle).Address);
            //series2.HeaderAddress = new ExcelAddress(Row_StatsHeader, column2, Row_StatsHeader, column2);
            series2.Header = series2name;
        }

        public IEnumerable<Metric> GetComputedMetrics(bool shownOnly)
        {
            return GetMetrics(shownOnly).Where(m => m.IsComputed);
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

    public class Metric
    {
        public Metric(int colNum, string displayName, bool isComputed = false, bool show = true)
        {
            this.ColNum = colNum;
            this.DisplayName = displayName;
            this.IsComputed = isComputed;
            this.Show = show;
        }

        public int ColNum { get; set; }
        public string DisplayName { get; set; }
        public bool IsComputed { get; set; }
        public bool Show { get; set; }
    }
}
