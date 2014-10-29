using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;

namespace DAGenerators.Spreadsheets
{
    public class SearchReport_ScholasticTeacherExpress : SearchReportPPC
    {
        protected override void Setup()
        {
            TemplateFilename = "SearchTemplate_ScholasticTeacherExpress.xlsx";

            Metric_Revenue = new Metric(3, "Revenue");
            Metric_Cost = new Metric(4, "Cost");
            Metric_ROAS = new Metric(5, "ROAS");
            Metric_Orders = new Metric(6, "Orders");
            Metric_Impressions = new Metric(7, "Impressions");
            Metric_Clicks = new Metric(8, "Clicks");
            Metric_CTR = new Metric(9, "CTR");
            Metric_OrderRate = new Metric(10, "Order Rate");
            Metric_CPC = new Metric(11, "CPC");
            Metric_CPO = new Metric(12, "CPO");

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

    }
}
