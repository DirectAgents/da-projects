using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace DAGenerators.Spreadsheets
{
    //TODO: make a SearchReportECom (or ...Retail) that inherits from this
    public class SearchReportPPC : SpreadsheetBase
    {
        private const int Row_SummaryDate = 8;
        //private const int Row_StatsHeader = 11;
        private const int Row_ClientNameBottom = 24;
        private const int Row_Charts = 27;

        private const int Col_LeftChart = 2;
        private const int Col_RightChart = 9;
        private const int ChartWidth = 590;
        private const int ChartHeight = 280;

        protected string TemplateFilename = "SearchPPCtemplate.xlsx";
        private const int NumRows_SectionHeader = 2;

        private const int StartRow_Weekly = 12;
        private const int StartRow_Monthly = 16;
        protected bool WeeklyFirst
        {
            get { return StartRow_Weekly <= StartRow_Monthly; }
        }
        protected int StartRow_YearOverYear = 20;

        private const int StartRow_WeeklyCampaignPerfTemplate = 45;
        private const int StartRow_MonthlyCampaignPerfTemplate = 51;
        private const int NumRows_CampaignPerfTemplate = 3;
        private const int NumRows_CampaignPerfSubTemplate = 2;

        public MetricsHolder Metrics;
        protected ExcelWorksheet WS { get { return this.ExcelPackage.Workbook.Worksheets[1]; } }

        protected int NumWeeksAdded { get; set; }
        protected int NumMonthsAdded { get; set; }
        protected int NumWeekRowsAdded { get; set; }  // not including template rows
        protected int NumMonthRowsAdded { get; set; } // not including template rows

        protected int NumCampaignPerfWeeksAdded { get; set; }
        protected int NumCampaignPerfMonthsAdded { get; set; }

        public SearchReportPPC()
        {
            Metrics = new MetricsHolder();
        }

        public void Setup(string templateFolder)
        {
            Setup();
            var fileInfo = new FileInfo(Path.Combine(templateFolder, TemplateFilename));
            this.ExcelPackage = new ExcelPackage(fileInfo);

            SetReportDate(DateTime.Today);
            //SetColumnHeaders(); // do this after specifying which metrics are "shown"
        }

        protected virtual void Setup()
        {
            //TODO: a way for the caller to specify different property names
            Metrics.Title = new Metric(2, null) { PropName = "Title" };
            Metrics.Clicks = new Metric(3, "Clicks") { PropName = "Clicks" };
            Metrics.Impressions = new Metric(4, "Impressions") { PropName = "Impressions" };
            Metrics.CTR = new Metric(5, "CTR");
            Metrics.Cost = new Metric(6, "Spend") { PropName = "Cost" };
            Metrics.CPC = new Metric(7, "CPC");
            Metrics.Orders = new Metric(8, "Orders") { PropName = "Orders" };
            Metrics.Revenue = new Metric(9, "Revenue") { PropName = "Revenue" };
            Metrics.Net = new Metric(10, "Net");
            Metrics.ViewThrus = new Metric(11, "ViewThrus") { PropName = "ViewThrus" };
            Metrics.ViewThruRev = new Metric(12, "ViewThruRev");
            Metrics.CassConvs = new Metric(13, "ClickAssistConv") { PropName = "CassConvs" };
            Metrics.CassConVal = new Metric(14, "CAC Val") { PropName = "CassConVal" };
            Metrics.CPO = new Metric(15, "Cost/Order");
            Metrics.OrderRate = new Metric(16, "Order Rate");
            Metrics.RevPerOrder = new Metric(17, "Rev/Order");
            Metrics.ROAS = new Metric(18, "ROAS");
        }

        // Do this after setting up the columns
        public void MakeColumnHidden(Metric metric)
        {
            WS.Column(metric.ColNum).Hidden = true;
            //Note: the column will still exist in the spreadsheet; it will be hidden... the user can unhide it
        }

        public virtual void SetReportDate(DateTime date)
        {
            WS.Cells[Row_SummaryDate, 2].Value = "Report Summary - through " + date.ToShortDateString();
        }
        public virtual void SetReportingPeriod(DateTime fromDate, DateTime toDate)
        {
        }

        //public void SetColumnHeaders()
        //{
        //    var metrics = GetMetrics(true);
        //    foreach (var metric in metrics)
        //    {
        //        WS.Cells[Row_StatsHeader, metric.ColNum].Value = metric.DisplayName;
        //    }
        //    // Q: What about the headers under Monthly, YoY, Weekly Perf...?
        //}

        public virtual void SetClientName(string clientName)
        {
            WS.Cells[Row_ClientNameBottom + NumWeekRowsAdded + NumMonthRowsAdded, 2].Value = "Direct Agents | " + clientName.ToUpper();
        }

        public virtual void LoadWeeklyStats<T>(IEnumerable<T> stats)
        {
            int blankRows = 1;
            int startRow = StartRow_Weekly + (WeeklyFirst ? 0 : NumMonthRowsAdded);
            NumWeekRowsAdded = LoadStats(Metrics, WS, startRow, stats, blankRows);
            NumWeeksAdded = stats.Count();
            if (NumWeeksAdded == 0)
            {   // Delete the entire section, including blank row above
                int rowsToDelete = NumRows_SectionHeader + 1;
                WS.DeleteRowZ(startRow - rowsToDelete, rowsToDelete);
                NumWeekRowsAdded -= rowsToDelete;
            }
        }
        public virtual void LoadMonthlyStats<T>(IEnumerable<T> stats)
        {
            int blankRows = 1;
            int startRow = StartRow_Monthly + (WeeklyFirst ? NumWeekRowsAdded : 0);
            NumMonthRowsAdded = LoadStats(Metrics, WS, startRow, stats, blankRows);
            NumMonthsAdded = stats.Count();
            if (NumMonthsAdded == 0)
            {   // Delete the entire section, including blank row above
                int rowsToDelete = NumRows_SectionHeader + 1;
                WS.DeleteRowZ(startRow - rowsToDelete, rowsToDelete);
                NumMonthRowsAdded -= rowsToDelete;
            }
        }

        // (for the most recently completed month)
        // IEnumerable<T> stats should have two elements: last year's and this year's stats
        public virtual void LoadYearOverYearStats<T>(IEnumerable<T> stats)
        {
            int startRow = StartRow_YearOverYear + NumWeekRowsAdded + NumMonthRowsAdded;
            LoadStats(Metrics, WS, startRow, stats, 2);

            //YoY diff row...
            string diffFormula = "IFERROR((R[-1]C-R[-2]C)/R[-2]C,\"-\")";
            var metrics = Metrics.GetAll(false);
            foreach (var metric in metrics)
            {
                WS.Cells[startRow + 2, metric.ColNum].FormulaR1C1 = diffFormula;
            }
        }

        // Note: Load monthly CampaignPerfStats first, then weekly.
        // Load weeks/months in this order: latest, second-latest, etc...
        public void LoadMonthlyCampaignPerfStats<T>(Dictionary<string, IEnumerable<T>> campaignStatsDict, bool collapse, DateTime monthStart, DateTime monthEnd)
        {
            int startRowTemplate = StartRow_MonthlyCampaignPerfTemplate + NumWeekRowsAdded + NumMonthRowsAdded;
            LoadCampaignPerfStats<T>(campaignStatsDict, collapse, monthStart, monthEnd, startRowTemplate, true);
        }
        public void LoadWeeklyCampaignPerfStats<T>(Dictionary<string, IEnumerable<T>> campaignStatsDict, bool collapse, DateTime weekStart, DateTime weekEnd)
        {
            int startRowTemplate = StartRow_WeeklyCampaignPerfTemplate + NumWeekRowsAdded + NumMonthRowsAdded;
            LoadCampaignPerfStats<T>(campaignStatsDict, collapse, weekStart, weekEnd, startRowTemplate, false);
        }
        // Load stats for one week/month (with subcategories: channels/searchaccounts)
        // campaignStatsDict should have one key for each Channel/SearchAccount
        public void LoadCampaignPerfStats<T>(Dictionary<string, IEnumerable<T>> campaignStatsDict, bool collapse, DateTime startDate, DateTime endDate,
                                             int startRowTemplate, bool monthlyNotWeekly)
        {
            int startRowStats = startRowTemplate + NumRows_CampaignPerfTemplate; // start below the template
            int numRowsAdded = 0;
            var nonComputedMetrics = Metrics.GetNonComputed(false);
            string sumFormula;

            // Insert rows (below the template)
            WS.InsertRowZ(startRowStats, NumRows_CampaignPerfTemplate, startRowTemplate);

            // Copy template rows (and paste just below them)
            WS.Cells[startRowTemplate + ":" + (startRowTemplate + NumRows_CampaignPerfTemplate - 1)]
                .Copy(WS.Cells[startRowStats + ":" + (startRowStats + NumRows_CampaignPerfTemplate - 1)]);

            // Now we use the "copy" to fill in stats for this week/month. The template remains above.

            // Populate the rows for each Subgroup (Channel/SearchAccount)...
            var subgroupSummaryOffsets = new List<int>(); // used to generate grand total sums
            int totalSubgroupRows = 0;
            int startRowSubgroupTemplate = startRowStats;
            int startRowSubgroupStats = startRowSubgroupTemplate + NumRows_CampaignPerfSubTemplate; // (below the template)
            var subgroupKeys = campaignStatsDict.Keys.Where(k => k != "Total").OrderBy(k => k == "Google").ThenByDescending(k => k);
            foreach (string subgroupKey in subgroupKeys) // in reverse order (so we end up with Google, then others alphabetically)
            {
                int numCampaigns = campaignStatsDict[subgroupKey].Count();
                bool lastSubgroup = (subgroupKey == subgroupKeys.Last());

                if (lastSubgroup)
                {
                    // use the template for the last subgroup
                    startRowSubgroupStats = startRowSubgroupTemplate;
                }
                else
                {
                    // insert rows and make a copy of the template
                    WS.InsertRowZ(startRowSubgroupStats, NumRows_CampaignPerfSubTemplate, startRowSubgroupTemplate);
                    numRowsAdded += NumRows_CampaignPerfSubTemplate;

                    WS.Cells[startRowSubgroupTemplate + ":" + (startRowSubgroupTemplate + NumRows_CampaignPerfSubTemplate - 1)]
                        .Copy(WS.Cells[startRowSubgroupStats + ":" + (startRowSubgroupStats + NumRows_CampaignPerfSubTemplate - 1)]);
                }

                // populate the campaign rows for this subgroup
                numRowsAdded += LoadStats(Metrics, WS, startRowSubgroupStats, campaignStatsDict[subgroupKey], NumRows_CampaignPerfSubTemplate - 1);

                // set the subgroup title
                int subgroupTotalRow = startRowSubgroupStats + numCampaigns;
                WS.Cells[subgroupTotalRow, 2].Value = subgroupKey;

                // fill in subgroup sum formulas
                sumFormula = String.Format("SUM(R[{0}]C:R[{1}]C)", -numCampaigns, -1);
                foreach (var metric in nonComputedMetrics)
                {
                    WS.Cells[subgroupTotalRow, metric.ColNum].FormulaR1C1 = sumFormula;
                }

                subgroupSummaryOffsets.Add(-1 - totalSubgroupRows);
                totalSubgroupRows += numCampaigns + 1; // add one for the subgroup summary row
            }

            // Populate grand total row
            string grandTotalLabel;
            if (monthlyNotWeekly)
            {
                DateTime fullMonthEnd = startDate.AddMonths(1).AddDays(-1);
                string extra = (endDate == fullMonthEnd) ? "" : " (partial)";
                grandTotalLabel = startDate.ToString("MMMM yyyy") + " TOTALS" + extra;
            }
            else
            {
                grandTotalLabel = String.Format("{0:M/d} - {1:M/d} TOTALS", startDate, endDate);
            }
            int grandTotalRow = startRowStats + totalSubgroupRows;
            WS.Cells[grandTotalRow, 2].Value = grandTotalLabel;

            // grand total row sums
            subgroupSummaryOffsets.Reverse();
            sumFormula = "SUM(" + String.Join(",", subgroupSummaryOffsets.Select(offset => String.Format("R[{0}]C", offset))) + ")";
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

            if (monthlyNotWeekly)
                NumCampaignPerfMonthsAdded++;
            else
                NumCampaignPerfWeeksAdded++;
        }

        public void CampaignPerfStatsCleanup(bool monthlyNotWeekly)
        {
            int startRowTemplate = (monthlyNotWeekly ? StartRow_MonthlyCampaignPerfTemplate : StartRow_WeeklyCampaignPerfTemplate)
                    + NumWeekRowsAdded + NumMonthRowsAdded;
            WS.DeleteRowZ(startRowTemplate, NumRows_CampaignPerfTemplate);

            bool deleteSection = monthlyNotWeekly ? (NumCampaignPerfMonthsAdded == 0) : (NumCampaignPerfWeeksAdded == 0);
            if (deleteSection)
            {   // Delete the entire section, including blank row above
                int rowsToDelete = NumRows_SectionHeader + 1;
                WS.DeleteRowZ(startRowTemplate - rowsToDelete, rowsToDelete);
            }
            // TODO? Update drawings and ranges - like in InsertRowZ() ?
        }

        public virtual void CreateCharts(bool weeklyNotMonthly)
        {
            CreateChart(Metrics.Title.ColNum, Metrics.Revenue, Metrics.ROAS, false, weeklyNotMonthly);
            CreateChart(Metrics.Title.ColNum, Metrics.Orders, Metrics.CPO, true, weeklyNotMonthly);
        }
        protected void CreateChart(int titleCol, Metric metric1, Metric metric2, bool rightSide, bool weeklyNotMonthly)
        {
            int topRow = Row_Charts + NumWeekRowsAdded + NumMonthRowsAdded - 1;
            int leftCol = (rightSide ? Col_RightChart : Col_LeftChart) - 1;
            string chartNameSuffix = (rightSide ? "Right" : "Left");

            int startRow_Stats, numRows_Stats;
            string typeWM;
            if (weeklyNotMonthly)
            {
                startRow_Stats = StartRow_Weekly + (WeeklyFirst ? 0 : NumMonthRowsAdded);
                numRows_Stats = NumWeeksAdded;
                typeWM = "Weekly";
            }
            else
            {
                startRow_Stats = StartRow_Monthly + (WeeklyFirst ? NumWeekRowsAdded : 0);
                numRows_Stats = NumMonthsAdded;
                typeWM = "Monthly";
            }
            CreateChart(WS, titleCol, metric1, metric2, startRow_Stats, numRows_Stats, topRow, leftCol, ChartWidth, ChartHeight, typeWM, chartNameSuffix);
        }

    }

    public class MetricsHolder : MetricsHolderBase
    {
        // Non-computed...
        public Metric Title;
        public Metric Impressions, Clicks, Orders, Cost, Revenue, Calls, ViewThrus, CassConvs, CassConVal;

        // Computed...
        public Metric OrderRate, Net, RevPerOrder, CTR, CPC, CPO, ROAS, ROI, TotalLeads, CPL, ViewThruRev;

        public override IEnumerable<Metric> GetNonComputed(bool includeTitle)
        {
            var metrics = new Metric[]
            {
                Impressions, Clicks, Orders, Cost, Revenue, Calls, ViewThrus, CassConvs, CassConVal
            };
            var ncMetrics = metrics.Where(m => m != null);
            if (includeTitle && Title != null)
                ncMetrics = (new Metric[] { Title }).Concat(ncMetrics);
            return ncMetrics;
        }

        public override IEnumerable<Metric> GetComputed()
        {
            var metrics = new Metric[]
            {
                OrderRate, Net, RevPerOrder, CTR, CPC, CPO, ROAS, ROI, TotalLeads, CPL, ViewThruRev
            };
            return metrics.Where(m => m != null);
        }
    }
}
