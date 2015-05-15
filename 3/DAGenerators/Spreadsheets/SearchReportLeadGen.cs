
namespace DAGenerators.Spreadsheets
{
    public class SearchReportLeadGen : SearchReportPPC
    {
        protected override void Setup()
        {
            TemplateFilename = "SearchTemplateLeadGen.xlsx";

            Metrics.Title = new Metric(2, null) { PropName = "Title" };
            Metrics.Clicks = new Metric(3, "Clicks") { PropName = "Clicks" };
            Metrics.Impressions = new Metric(4, "Impressions") { PropName = "Impressions" };
            Metrics.CTR = new Metric(5, "CTR");
            Metrics.Cost = new Metric(6, "Spend") { PropName = "Cost" };
            Metrics.CPC = new Metric(7, "CPC");
            Metrics.Orders = new Metric(8, "Leads") { PropName = "Orders" };
            Metrics.CPL = new Metric(9, "CPL");
            Metrics.OrderRate = new Metric(10, "Conv Rate");
        }

        public override void CreateCharts(bool weeklyNotMonthly)
        {
            CreateChart(Metrics.Title.ColNum, Metrics.CTR, Metrics.CPC, false, weeklyNotMonthly);
            CreateChart(Metrics.Title.ColNum, Metrics.Orders, Metrics.CPL, true, weeklyNotMonthly);
        }
    }

    public class SearchReportLeadGenWithCalls : SearchReportPPC
    {
        protected override void Setup()
        {
            TemplateFilename = "SearchTemplateLeadGenWithCalls.xlsx";

            Metrics.Title = new Metric(2, null) { PropName = "Title" };
            Metrics.Clicks = new Metric(3, "Clicks") { PropName = "Clicks" };
            Metrics.Impressions = new Metric(4, "Impressions") { PropName = "Impressions" };
            Metrics.CTR = new Metric(5, "CTR");
            Metrics.Cost = new Metric(6, "Spend") { PropName = "Cost" };
            Metrics.CPC = new Metric(7, "CPC");
            Metrics.Orders = new Metric(8, "Leads") { PropName = "Orders" };
            Metrics.Calls = new Metric(9, "Calls") { PropName = "Calls" };
            Metrics.TotalLeads = new Metric(10, "Total Leads");
            Metrics.CPL = new Metric(11, "CPL");
            Metrics.OrderRate = new Metric(12, "Conv Rate");
        }

        public override void CreateCharts(bool weeklyNotMonthly)
        {
            CreateChart(Metrics.Title.ColNum, Metrics.CTR, Metrics.CPC, false, weeklyNotMonthly);
            CreateChart(Metrics.Title.ColNum, Metrics.TotalLeads, Metrics.CPL, true, weeklyNotMonthly);
        }
    }
}
