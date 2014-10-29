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
    public partial class SearchReportPPC : SpreadsheetBase
    {
        private const int Row_SummaryDate = 8;
        private const int Row_StatsHeader = 11;
        private const int Row_ClientNameBottom = 14;
        private const int Row_WeeklyChart = 15;

        private const int Col_StatsTitle = 2;

        private string TemplateFilename = "SearchPPCtemplate.xlsx";
        private int StartRow_Weekly = 12;
        private int StartRow_Monthly = 13;
        private bool WeeklyFirst
        {
            get { return StartRow_Weekly <= StartRow_Monthly; }
        }

        public Metric Metric_Clicks = new Metric(0, null, false);
        public Metric Metric_Impressions = new Metric(0, null, false);
        public Metric Metric_Orders = new Metric(0, null, false);
        public Metric Metric_Cost = new Metric(0, null, false);
        public Metric Metric_Revenue = new Metric(0, null, false);

        // Computed metrics
        public Metric Metric_OrderRate = new Metric(0, null, false);
        public Metric Metric_Net = new Metric(0, null, false);
        public Metric Metric_RevPerOrder = new Metric(0, null, false);
        public Metric Metric_CTR = new Metric(0, null, false);
        public Metric Metric_CPC = new Metric(0, null, false);
        public Metric Metric_CPO = new Metric(0, null, false);
        public Metric Metric_ROAS = new Metric(0, null, false);
        public Metric Metric_ROI = new Metric(0, null, false);

        ExcelWorksheet WS { get { return this.ExcelPackage.Workbook.Worksheets[1]; } }
        int NumWeeksAdded { get; set; }
        int NumMonthsAdded { get; set; }

        public SearchReportPPC(string templateFolder, string templateName = null)
        {
            Setup(templateName);
            var fileInfo = new FileInfo(Path.Combine(templateFolder, TemplateFilename));
            this.ExcelPackage = new ExcelPackage(fileInfo);

            SetReportDate(DateTime.Today);
            SetColumnHeaders(); // do this after showing/hiding metrics
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

        public void SetClientName(string clientName)
        {
            WS.Cells[Row_ClientNameBottom + NumWeeksAdded + NumMonthsAdded, 2].Value = "Direct Agents | " + clientName;
        }

        public void LoadWeeklyStats<T>(IEnumerable<T> stats, IList<string> propertyNames)
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
        private void LoadWeeklyMonthlyStats<T>(IEnumerable<T> stats, IList<string> propertyNames, int startingRow)
        {
            int numRows = stats.Count();
            if (numRows > 0)
            {
                WS.InsertRow(startingRow, numRows, startingRow); // # rows inserted == size of the stats enumerable

                LoadColumnFromStats(stats, startingRow, Col_StatsTitle, propertyNames[0]);
                LoadColumnFromStats(stats, startingRow, Metric_Clicks.ColNum, propertyNames[1], Metric_Clicks);
                LoadColumnFromStats(stats, startingRow, Metric_Impressions.ColNum, propertyNames[2], Metric_Impressions);
                LoadColumnFromStats(stats, startingRow, Metric_Orders.ColNum, propertyNames[3], Metric_Orders);
                LoadColumnFromStats(stats, startingRow, Metric_Cost.ColNum, propertyNames[4], Metric_Cost);
                LoadColumnFromStats(stats, startingRow, Metric_Revenue.ColNum, propertyNames[5], Metric_Revenue);

                for (int i = 0; i < numRows; i++)
                {
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
}
