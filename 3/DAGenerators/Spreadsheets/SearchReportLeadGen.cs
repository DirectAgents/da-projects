
namespace DAGenerators.Spreadsheets
{
    public class SearchReportLeadGen : SearchReportPPC
    {
        protected override void Setup()
        {
            TemplateFilename = "SearchTemplateLeadGen.xlsx";

            Metrics1.Title = new Metric(2, null) { MemberName = "Title" };
            Metrics1.Clicks = new Metric(3, "Clicks") { MemberName = "Clicks_" };
            Metrics1.Impressions = new Metric(4, "Impressions") { MemberName = "Impressions_" };
            Metrics1.CTR = new Metric(5, "CTR");
            Metrics1.Cost = new Metric(6, "Spend") { MemberName = "Cost_" };
            Metrics1.CPC = new Metric(7, "CPC");
            Metrics1.Orders = new Metric(8, "Leads") { MemberName = "Orders_" };
            Metrics1.CPL = new Metric(9, "CPL");
            Metrics1.OrderRate = new Metric(10, "Conv Rate");
        }

        public override void CreateCharts(bool weeklyNotMonthly)
        {
            CreateChart(Metrics1.Title.ColNum, Metrics1.CTR, Metrics1.CPC, false, weeklyNotMonthly);
            CreateChart(Metrics1.Title.ColNum, Metrics1.Orders, Metrics1.CPL, true, weeklyNotMonthly);
        }
    }

    public class SearchReportLeadGenWithCalls : SearchReportPPC
    {
        protected override void Setup()
        {
            TemplateFilename = "SearchTemplateLeadGenWithCalls.xlsx";

            Metrics1.Title = new Metric(2, null) { MemberName = "Title" };
            Metrics1.Clicks = new Metric(3, "Clicks") { MemberName = "Clicks_" };
            Metrics1.Impressions = new Metric(4, "Impressions") { MemberName = "Impressions_" };
            Metrics1.CTR = new Metric(5, "CTR");
            Metrics1.Cost = new Metric(6, "Spend") { MemberName = "Cost_" };
            Metrics1.CPC = new Metric(7, "CPC");
            Metrics1.Orders = new Metric(8, "Leads") { MemberName = "Orders_" };
            Metrics1.Calls = new Metric(9, "Calls") { MemberName = "Calls_" };
            Metrics1.TotalLeads = new Metric(10, "Total Leads_");
            Metrics1.CPL = new Metric(11, "CPL");
            Metrics1.OrderRate = new Metric(12, "Conv Rate");
        }

        public override void CreateCharts(bool weeklyNotMonthly)
        {
            CreateChart(Metrics1.Title.ColNum, Metrics1.CTR, Metrics1.CPC, false, weeklyNotMonthly);
            CreateChart(Metrics1.Title.ColNum, Metrics1.TotalLeads, Metrics1.CPL, true, weeklyNotMonthly);
        }
    }
}
