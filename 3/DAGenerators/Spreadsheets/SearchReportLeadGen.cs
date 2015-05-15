
namespace DAGenerators.Spreadsheets
{
    public class SearchReportLeadGen : SearchReportPPC
    {
        protected override void Setup()
        {
            TemplateFilename = "SearchTemplateLeadGen.xlsx";

            Metrics.Clicks = new Metric(3, "Clicks");
            Metrics.Impressions = new Metric(4, "Impressions");
            Metrics.CTR = new Metric(5, "CTR", true);
            Metrics.Cost = new Metric(6, "Spend");
            Metrics.CPC = new Metric(7, "CPC", true);
            Metrics.Orders = new Metric(8, "Leads");
            Metrics.CPL = new Metric(9, "CPL", true);
            Metrics.OrderRate = new Metric(10, "Conv Rate", true);
        }

        public override void CreateCharts(bool weeklyNotMonthly)
        {
            CreateChart(Metrics.CTR, Metrics.CPC, false, weeklyNotMonthly);
            CreateChart(Metrics.Orders, Metrics.CPL, true, weeklyNotMonthly);
        }
    }

    public class SearchReportLeadGenWithCalls : SearchReportPPC
    {
        protected override void Setup()
        {
            TemplateFilename = "SearchTemplateLeadGenWithCalls.xlsx";

            Metrics.Clicks = new Metric(3, "Clicks");
            Metrics.Impressions = new Metric(4, "Impressions");
            Metrics.CTR = new Metric(5, "CTR", true);
            Metrics.Cost = new Metric(6, "Spend");
            Metrics.CPC = new Metric(7, "CPC", true);
            Metrics.Orders = new Metric(8, "Leads");
            Metrics.Calls = new Metric(9, "Calls");
            Metrics.TotalLeads = new Metric(10, "Total Leads", true);
            Metrics.CPL = new Metric(11, "CPL", true);
            Metrics.OrderRate = new Metric(12, "Conv Rate", true);
        }

        public override void CreateCharts(bool weeklyNotMonthly)
        {
            CreateChart(Metrics.CTR, Metrics.CPC, false, weeklyNotMonthly);
            CreateChart(Metrics.TotalLeads, Metrics.CPL, true, weeklyNotMonthly);
        }
    }
}
