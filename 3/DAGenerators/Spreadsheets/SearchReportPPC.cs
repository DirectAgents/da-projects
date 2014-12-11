using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DAGenerators.Spreadsheets
{
    //TODO: make a SearchReportECom (or ...Retail) that inherits from this
    public class SearchReportPPC : SpreadsheetBase
    {
        protected int Row_SummaryDate = 8;
        protected int Row_StatsHeader = 11;
        private const int Row_ClientNameBottom = 18;
        private const int Row_WeeklyChart = 21;

        private const int Col_StatsTitle = 2;
        private const int Col_LeftChart = 2;
        private const int Col_RightChart = 9;
        private const int ChartWidth = 590;
        private const int ChartHeight = 280;

        protected string TemplateFilename = "SearchPPCtemplate.xlsx";
        protected int StartRow_Weekly = 12;
        protected int StartRow_Monthly = 16;
        protected bool WeeklyFirst
        {
            get { return StartRow_Weekly <= StartRow_Monthly; }
        }

        private const int StartRow_WeeklyChannelCampaignTemplate = 39;
        private const int NumRows_WeeklyChannelCampaignTemplate = 3;
        private const int NumRows_WeeklyChannelRollupTemplate = 2;

        public Metric Metric_Clicks = new Metric(0, null, false, false);
        public Metric Metric_Impressions = new Metric(0, null, false, false);
        public Metric Metric_Orders = new Metric(0, null, false, false);
        public Metric Metric_Cost = new Metric(0, null, false, false);
        public Metric Metric_Revenue = new Metric(0, null, false, false);
        public Metric Metric_Calls = new Metric(0, null, false, false);

        // Computed metrics
        public Metric Metric_OrderRate = new Metric(0, null, true, false);
        public Metric Metric_Net = new Metric(0, null, true, false);
        public Metric Metric_RevPerOrder = new Metric(0, null, true, false);
        public Metric Metric_CTR = new Metric(0, null, true, false);
        public Metric Metric_CPC = new Metric(0, null, true, false);
        public Metric Metric_CPO = new Metric(0, null, true, false);
        public Metric Metric_ROAS = new Metric(0, null, true, false);
        public Metric Metric_ROI = new Metric(0, null, true, false);
        public Metric Metric_TotalLeads = new Metric(0, null, true, false);
        public Metric Metric_CPL = new Metric(0, null, true, false);

        protected ExcelWorksheet WS { get { return this.ExcelPackage.Workbook.Worksheets[1]; } }
        protected int NumWeeksAdded { get; set; }
        protected int NumWeekRowsAdded { get; set; }  // not including template rows
        protected int NumMonthRowsAdded { get; set; } // not including template rows

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
            Metric_CTR = new Metric(5, "CTR", true);
            Metric_Cost = new Metric(6, "Spend");
            Metric_CPC = new Metric(7, "CPC", true);
            Metric_Orders = new Metric(8, "Orders");
            Metric_Revenue = new Metric(9, "Revenue");
            Metric_Net = new Metric(10, "Net", true);
            Metric_CPO = new Metric(11, "Cost/Order", true);
            Metric_OrderRate = new Metric(12, "Order Rate", true);
            Metric_RevPerOrder = new Metric(13, "Rev/Order", true);
            Metric_ROAS = new Metric(14, "ROAS", true);
        }

        public virtual void SetReportDate(DateTime date)
        {
            WS.Cells[Row_SummaryDate, 2].Value = "Report Summary - through " + date.ToShortDateString();
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
            WS.Cells[Row_ClientNameBottom + NumWeekRowsAdded + NumMonthRowsAdded, 2].Value = "Direct Agents | " + clientName.ToUpper();
        }

        public virtual void LoadWeeklyStats<T>(IEnumerable<T> stats, IList<string> propertyNames)
        {
            NumWeekRowsAdded += LoadWeeklyMonthlyStats(stats, propertyNames, StartRow_Weekly + (WeeklyFirst ? 0 : NumMonthRowsAdded), 1);
            NumWeeksAdded += stats.Count();
            CreateWeeklyCharts();
        }
        public virtual void CreateWeeklyCharts()
        {
            //CreateWeeklyChart(NumWeeksAdded, Metric_Revenue, Metric_Clicks);
            CreateWeeklyChart(NumWeeksAdded, Metric_Revenue, Metric_ROAS);
            CreateWeeklyChart(NumWeeksAdded, Metric_Orders, Metric_CPO, true);
        }

        public virtual void LoadMonthlyStats<T>(IEnumerable<T> stats, IList<string> propertyNames)
        {
            NumMonthRowsAdded += LoadWeeklyMonthlyStats(stats, propertyNames, StartRow_Monthly + (WeeklyFirst ? NumWeekRowsAdded : 0), 1);
        }
        //public void LoadMonthlyCampaignStats<T>(IEnumerable<T> stats, IList<string> propertyNames)
        //{
        //    LoadWeeklyMonthlyStats(stats, propertyNames, StartRow_Monthly + nummon
        //}

        // propertyNames for: title, clicks, impressions, orders, cost, revenue, calls
        // returns: # of rows added (in addition to blankRowsInTemplate); negative means blankRows deleted
        protected int LoadWeeklyMonthlyStats<T>(IEnumerable<T> stats, IList<string> propertyNames, int startingRow, int blankRowsInTemplate = 0)
        {
            int numRows = stats.Count();
            int blankRowsToInsert = numRows - blankRowsInTemplate;
            if (blankRowsToInsert < 0)
            {
                WS.DeleteRowZ(startingRow, -blankRowsToInsert);
            }
            if (numRows > 0)
            {
                if (blankRowsToInsert > 0)
                {
                    WS.InsertRowZ(startingRow + (blankRowsInTemplate > 0 ? 1 : 0), blankRowsToInsert, startingRow);

                    if (blankRowsInTemplate > 0)
                    {   // copy the formulas from the "blank row" to the newly inserted rows
                        for (int iRow = startingRow + 1; iRow < startingRow + 1 + blankRowsToInsert; iRow++)
                        {
                            WS.Cells[startingRow + ":" + startingRow].Copy(WS.Cells[iRow + ":" + iRow]);
                        }
                    }
                    else
                    {   // generate the formulas if there were no blank rows
                        for (int i = 0; i < numRows; i++)
                            LoadStatsRowFormulas(startingRow + i);
                    }
                }

                LoadColumnFromStats(stats, startingRow, Col_StatsTitle, propertyNames[0]);
                LoadColumnFromStats(stats, startingRow, Metric_Clicks.ColNum, propertyNames[1], Metric_Clicks);
                LoadColumnFromStats(stats, startingRow, Metric_Impressions.ColNum, propertyNames[2], Metric_Impressions);
                LoadColumnFromStats(stats, startingRow, Metric_Orders.ColNum, propertyNames[3], Metric_Orders);
                LoadColumnFromStats(stats, startingRow, Metric_Cost.ColNum, propertyNames[4], Metric_Cost);
                LoadColumnFromStats(stats, startingRow, Metric_Revenue.ColNum, propertyNames[5], Metric_Revenue);
                LoadColumnFromStats(stats, startingRow, Metric_Calls.ColNum, propertyNames[6], Metric_Calls);
            }
            return blankRowsToInsert;
        }
        private void LoadColumnFromStats<T>(IEnumerable<T> stats, int startingRow, int iColumn, string propertyName, Metric metric = null)
        {
            if (metric == null || metric.Show)
            {
                var type = stats.First().GetType();
                WS.Cells[startingRow, iColumn].LoadFromCollection(stats, false, TableStyles.None, BindingFlags.Default, new[] { type.GetProperty(propertyName) });
            }
        }

        //TODO: retire this - if can assume all formulas are in template rows in the spreadsheet
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
            //CheckLoadStatsRowFormula(iRow, Metric_TotalLeads
            //CheckLoadStatsRowFormula(iRow, Metric_CPL
        }
        private void CheckLoadStatsRowFormula(int iRow, Metric metric, string formula)
        {
            if (metric.Show)
                WS.Cells[iRow, metric.ColNum].FormulaR1C1 = formula;
        }

        // Do in this order: latest, second-latest, etc...
        // channelStatsDict should have one key for each channel
        public void LoadWeeklyChannelRollupStats<T>(Dictionary<string, IEnumerable<T>> channelStatsDict, IList<string> propertyNames, DateTime weekStart, bool collapse)
        {
            int startRowTemplate = StartRow_WeeklyChannelCampaignTemplate + NumWeekRowsAdded + NumMonthRowsAdded;
            int startRowStats = startRowTemplate + NumRows_WeeklyChannelCampaignTemplate;
            int numRowsAdded = 0;
            var nonComputedMetrics = GetNonComputedMetrics(true);
            string sumFormula;

            // Insert rows (below the template rows)
            WS.InsertRowZ(startRowStats, NumRows_WeeklyChannelCampaignTemplate, startRowTemplate);

            // Copy template rows (and paste just below them)
            WS.Cells[startRowTemplate + ":" + (startRowTemplate + NumRows_WeeklyChannelCampaignTemplate - 1)]
                .Copy(WS.Cells[startRowStats + ":" + (startRowStats + NumRows_WeeklyChannelCampaignTemplate - 1)]);

            // Populate the rows for each channel...
            var channelSummaryOffsets = new List<int>(); // used to generate grand total sums
            int totalRollupRows = 0;
            int startRowChannelRollupTemplate = startRowStats;
            int startRowChannelRollup = startRowChannelRollupTemplate + NumRows_WeeklyChannelRollupTemplate;
            var channelKeys = channelStatsDict.Keys.Where(k => k != "Total").OrderBy(k => k == "Google").ThenByDescending(k => k);
            foreach (string channelKey in channelKeys) // in reverse order (so we end up with Google, then others alphabetically)
            {
                int numCampaigns = channelStatsDict[channelKey].Count();
                bool lastChannel = (channelKey == channelKeys.Last());

                if (lastChannel)
                {
                    // use the template for the last channel
                    startRowChannelRollup = startRowChannelRollupTemplate;
                }
                else
                {
                    // insert rows and make a copy of the template
                    WS.InsertRowZ(startRowChannelRollup, NumRows_WeeklyChannelRollupTemplate, startRowChannelRollupTemplate);
                    numRowsAdded += NumRows_WeeklyChannelRollupTemplate;

                    WS.Cells[startRowChannelRollupTemplate + ":" + (startRowChannelRollupTemplate + NumRows_WeeklyChannelRollupTemplate - 1)]
                        .Copy(WS.Cells[startRowChannelRollup + ":" + (startRowChannelRollup + NumRows_WeeklyChannelRollupTemplate - 1)]);
                }

                // populate the campaign rows for this channel
                numRowsAdded += LoadWeeklyMonthlyStats(channelStatsDict[channelKey], propertyNames, startRowChannelRollup, NumRows_WeeklyChannelRollupTemplate - 1);

                // set the channel title
                int channelTotalRow = startRowChannelRollup + numCampaigns;
                WS.Cells[channelTotalRow, 2].Value = channelKey;

                // fill in channel sum formulas
                sumFormula = String.Format("SUM(R[{0}]C:R[{1}]C)", -numCampaigns, -1);
                foreach (var metric in nonComputedMetrics)
                {
                    WS.Cells[channelTotalRow, metric.ColNum].FormulaR1C1 = sumFormula;
                }

                channelSummaryOffsets.Add(-1 - totalRollupRows);
                totalRollupRows += numCampaigns + 1; // add one for the channel summary row
            }

            // Populate grand total row
            int grandTotalRow = startRowStats + totalRollupRows;
            var weekEnd = weekStart.AddDays(6);
            WS.Cells[grandTotalRow, 2].Value = String.Format("{0:M/d} - {1:M/d} TOTALS", weekStart, weekEnd);

            // grand total row sums
            channelSummaryOffsets.Reverse();
            sumFormula = "SUM(" + String.Join(",", channelSummaryOffsets.Select(offset => String.Format("R[{0}]C", offset))) + ")";
            foreach (var metric in nonComputedMetrics)
            {
                WS.Cells[grandTotalRow, metric.ColNum].FormulaR1C1 = sumFormula;
            }

            // make rows collapsible
            for (int iRow = startRowStats; iRow < grandTotalRow; iRow++)
            {
                WS.Row(iRow).OutlineLevel = 1;
            }
            if (collapse)
            {
                for (int iRow = startRowStats; iRow < grandTotalRow; iRow++)
                    WS.Row(iRow).Collapsed = true;
            }
        }

        public void RemoveWeeklyChannelRollupStatsTemplate()
        {
            int startRowTemplate = StartRow_WeeklyChannelCampaignTemplate + NumWeekRowsAdded + NumMonthRowsAdded;
            WS.DeleteRowZ(startRowTemplate, NumRows_WeeklyChannelCampaignTemplate);
            // TODO? Update drawings and ranges - like in InsertRowZ() ?
        }


        protected void CreateWeeklyChart(int numWeeks, Metric metric1, Metric metric2, bool rightSide = false)
        {
            var chart = WS.Drawings.AddChart("chartWeekly" + (rightSide ? "Right" : "Left"), eChartType.ColumnClustered);
            chart.SetPosition(Row_WeeklyChart + NumWeekRowsAdded + NumMonthRowsAdded - 1, 0, (rightSide ? Col_RightChart : Col_LeftChart) - 1, 0); // row & column are 0-based
            chart.SetSize(ChartWidth, ChartHeight);

            chart.Title.Text = "Weekly " + metric1.DisplayName + " vs. " + metric2.DisplayName;
            chart.Title.Font.Bold = true;
            //chart.Title.Anchor = eTextAnchoringType.Bottom;
            //chart.Title.AnchorCtr = false;

            int startRow_Weekly = this.StartRow_Weekly + (WeeklyFirst ? 0 : NumMonthRowsAdded);

            var series = chart.Series.Add(new ExcelAddress(startRow_Weekly, metric1.ColNum, startRow_Weekly + numWeeks - 1, metric1.ColNum).Address,
                                          new ExcelAddress(startRow_Weekly, Col_StatsTitle, startRow_Weekly + numWeeks - 1, Col_StatsTitle).Address);
            //series.HeaderAddress = new ExcelAddress(Row_StatsHeader, column1, Row_StatsHeader, column1);
            series.Header = metric1.DisplayName;

            var chartType2 = chart.PlotArea.ChartTypes.Add(eChartType.LineMarkers);
            chartType2.UseSecondaryAxis = true;
            chartType2.XAxis.Deleted = true;
            var series2 = chartType2.Series.Add(new ExcelAddress(startRow_Weekly, metric2.ColNum, startRow_Weekly + numWeeks - 1, metric2.ColNum).Address,
                                                new ExcelAddress(startRow_Weekly, Col_StatsTitle, startRow_Weekly + numWeeks - 1, Col_StatsTitle).Address);
            //series2.HeaderAddress = new ExcelAddress(Row_StatsHeader, column2, Row_StatsHeader, column2);
            series2.Header = metric2.DisplayName;

            chart.Legend.Position = eLegendPosition.Bottom;
        }

        public IEnumerable<Metric> GetComputedMetrics(bool shownOnly)
        {
            return GetMetrics(shownOnly).Where(m => m.IsComputed);
        }
        public IEnumerable<Metric> GetNonComputedMetrics(bool shownOnly)
        {
            return GetMetrics(shownOnly).Where(m => !m.IsComputed);
        }
        public IEnumerable<Metric> GetMetrics(bool shownOnly)
        {
            var metrics = new List<Metric>();
            PossiblyAddMetric(metrics, Metric_Clicks, shownOnly);
            PossiblyAddMetric(metrics, Metric_Impressions, shownOnly);
            PossiblyAddMetric(metrics, Metric_Orders, shownOnly);
            PossiblyAddMetric(metrics, Metric_Cost, shownOnly);
            PossiblyAddMetric(metrics, Metric_Revenue, shownOnly);
            PossiblyAddMetric(metrics, Metric_Calls, shownOnly);

            PossiblyAddMetric(metrics, Metric_OrderRate, shownOnly);
            PossiblyAddMetric(metrics, Metric_Net, shownOnly);
            PossiblyAddMetric(metrics, Metric_RevPerOrder, shownOnly);
            PossiblyAddMetric(metrics, Metric_CTR, shownOnly);
            PossiblyAddMetric(metrics, Metric_CPC, shownOnly);
            PossiblyAddMetric(metrics, Metric_CPO, shownOnly);
            PossiblyAddMetric(metrics, Metric_ROAS, shownOnly);
            PossiblyAddMetric(metrics, Metric_ROI, shownOnly);
            PossiblyAddMetric(metrics, Metric_TotalLeads, shownOnly);
            PossiblyAddMetric(metrics, Metric_CPL, shownOnly);

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
