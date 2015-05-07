
namespace DAGenerators.Spreadsheets
{
    public class SearchReportLeadGen : SearchReportPPC
    {
        protected override void Setup()
        {
            TemplateFilename = "SearchTemplateLeadGen.xlsx";

            Metric_Clicks = new Metric(3, "Clicks");
            Metric_Impressions = new Metric(4, "Impressions");
            Metric_CTR = new Metric(5, "CTR", true);
            Metric_Cost = new Metric(6, "Spend");
            Metric_CPC = new Metric(7, "CPC", true);
            Metric_Orders = new Metric(8, "Leads");
            Metric_CPL = new Metric(9, "CPL", true);
            Metric_OrderRate = new Metric(10, "Conv Rate", true);
        }

        public override void CreateCharts(bool weeklyNotMonthly)
        {
            CreateChart(Metric_CTR, Metric_CPC, false, weeklyNotMonthly);
            CreateChart(Metric_Orders, Metric_CPL, true, weeklyNotMonthly);
        }
    }

    public class SearchReportLeadGenWithCalls : SearchReportPPC
    {
        protected override void Setup()
        {
            TemplateFilename = "SearchTemplateLeadGenWithCalls.xlsx";

            Metric_Clicks = new Metric(3, "Clicks");
            Metric_Impressions = new Metric(4, "Impressions");
            Metric_CTR = new Metric(5, "CTR", true);
            Metric_Cost = new Metric(6, "Spend");
            Metric_CPC = new Metric(7, "CPC", true);
            Metric_Orders = new Metric(8, "Leads");
            Metric_Calls = new Metric(9, "Calls");
            Metric_TotalLeads = new Metric(10, "Total Leads", true);
            Metric_CPL = new Metric(11, "CPL", true);
            Metric_OrderRate = new Metric(12, "Conv Rate", true);
        }

        public override void CreateCharts(bool weeklyNotMonthly)
        {
            CreateChart(Metric_CTR, Metric_CPC, false, weeklyNotMonthly);
            CreateChart(Metric_TotalLeads, Metric_CPL, true, weeklyNotMonthly);
        }
    }
}
