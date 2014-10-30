using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Linq;
using System.Collections.Generic;
using OfficeOpenXml.Drawing;

namespace DAGenerators.Spreadsheets
{
    public class SearchReport_ScholasticTeacherExpress : SearchReportPPC
    {
        const int StartRow_LatestMonthCampaigns = 14;
        const int StartRow_LatestWeekly1 = 21;
        const int StartRow_LatestWeekly0 = 24;

        protected int NumLatestMonthCampaignRowsAdded = 0; // not including preexisting blank rows

        protected override void Setup()
        {
            TemplateFilename = "SearchTemplate_ScholasticTeacherExpress.xlsx";

            Metric_Revenue = new Metric(3, "Revenue");
            Metric_Cost = new Metric(4, "Cost");
            Metric_ROAS = new Metric(5, "ROAS", true);
            Metric_Orders = new Metric(6, "Orders");
            Metric_Impressions = new Metric(7, "Impressions");
            Metric_Clicks = new Metric(8, "Clicks");
            Metric_CTR = new Metric(9, "CTR", true);
            Metric_OrderRate = new Metric(10, "Order Rate", true);
            Metric_CPC = new Metric(11, "CPC", true);
            Metric_CPO = new Metric(12, "CPO", true);

            Row_SummaryDate = 4;
            Row_StatsHeader = 8;
            StartRow_Monthly = 9;
            StartRow_Weekly = 28;
        }

        public override void SetReportDate(DateTime date)
        {
        }
        public override void SetReportingPeriod(DateTime fromDate, DateTime toDate)
        {
            WS.Cells[Row_SummaryDate, 3].Value = fromDate.ToShortDateString() + " - " + toDate.ToShortDateString();
        }

        public override void SetClientName(string clientName)
        {
        }

        public override void LoadWeeklyStats<T>(IEnumerable<T> stats, IList<string> propertyNames)
        {
        }

        public void LoadLatestMonthCampaignStats<T>(IEnumerable<T> stats, IList<string> propertyNames, DateTime monthStart)
        {
            LoadWeeklyMonthlyStats(stats, propertyNames, StartRow_LatestMonthCampaigns + NumMonthsAdded, 2);
            int numCampaigns = stats.Count();
            if (numCampaigns > 2)
                NumLatestMonthCampaignRowsAdded += numCampaigns;

            var monthName = monthStart.ToString("MMMM");
            var drawingsToUpdate = WS.Drawings.Where(d => d is ExcelShape && ((ExcelShape)d).Text.Contains("Latest Month"));
            foreach (var drawing in drawingsToUpdate)
            {
                var shape = (ExcelShape)drawing;
                shape.Text = shape.Text.Replace("Latest Month", monthName);
            }
        }

        // Do in this order: latest, second-latest, etc...
        public void LoadLatestWeekCampaignStats<T>(IEnumerable<T> stats, IList<string> propertyNames, DateTime weekStart, int whichLatestWeek = 0)
        {
            whichLatestWeek = Math.Abs(whichLatestWeek);
            if (whichLatestWeek > 1) return;

            int startRow = (whichLatestWeek == 0 ? StartRow_LatestWeekly0 : StartRow_LatestWeekly1) + NumMonthsAdded + NumLatestMonthCampaignRowsAdded;
            var weekEnd = weekStart.AddDays(6);
            WS.Cells[startRow + 2, 2].Value = String.Format("{0:M/d} - {1:M/d}", weekStart, weekEnd);

            LoadWeeklyMonthlyStats(stats, propertyNames, startRow, 2);
        }

    }
}
