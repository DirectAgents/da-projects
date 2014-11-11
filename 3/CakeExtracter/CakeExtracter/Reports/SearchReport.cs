using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using DAGenerators.Charts;
using DAGenerators.Spreadsheets;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;

namespace CakeExtracter.Reports
{
    class SearchReport : IReport
    {
        private readonly IClientPortalRepository cpRepo;
        private readonly SimpleReport simpleReport;

        public SearchReport(IClientPortalRepository cpRepo, SimpleReport simpleReport)
        {
            this.cpRepo = cpRepo;
            this.simpleReport = simpleReport;
        }

        public string Subject
        {
            get { return "Direct Agents Search Report"; }
        }

        public string Generate()
        {
            if (simpleReport.SearchProfile == null)
                throw new Exception("Cannot generate search report without a searchProfile");
            var searchProfile = simpleReport.SearchProfile;

            var fromDate = simpleReport.GetStatsStartDate();
            var toDate = simpleReport.GetStatsEndDate();
            var stat = this.cpRepo.GetSearchStats(searchProfile.SearchProfileId, fromDate, toDate);

            var template = new SearchReportRuntimeTextTemplate();
            template.AdvertiserName = searchProfile.SearchProfileName ?? "";
            template.Week = string.Format("{0} - {1}", fromDate.ToShortDateString(), toDate.ToShortDateString());
            template.Revenue = stat.Revenue;
            template.Cost = stat.Cost;
            template.ROAS = stat.ROAS;
            template.Margin = stat.Margin;
            template.Orders = stat.Orders;
            template.CPO = stat.CPO;

            template.AcctMgrName = "";
            template.AcctMgrEmail = "";

            var searchProfileContacts = searchProfile.SearchProfileContactsOrdered;
            if (searchProfileContacts.Count() > 0)
            {
                var contact = searchProfileContacts.First().Contact;
                template.AcctMgrName = contact.FullName ?? "";
                template.AcctMgrEmail = contact.Email ?? "";
            }

            string content = template.TransformText();

            return content;
        }

        private ChartBase ChartObj { get; set; }
        private SpreadsheetBase Spreadsheet { get; set; }

        public AlternateView GenerateView()
        {
            var contentId = "chart1";
            var reportString = this.Generate();                                        //TODO: put in template?
            var htmlView = AlternateView.CreateAlternateViewFromString(reportString + "<table width=\"100%\"><tr><td style=\"text-align:center\"><img src=cid:" + contentId + "></td></tr></table>", null, "text/html");

            GenerateChart();
            var linkedResource = this.ChartObj.GetAsLinkedResource(contentId);
            htmlView.LinkedResources.Add(linkedResource);

            return htmlView;
        }

        private void GenerateChart() // hard-coded to an Orders/CPO chart for now
        {
            var searchProfile = simpleReport.SearchProfile;
            int numWeeks = 8;
            var toDate = simpleReport.GetStatsEndDate();
            var stats = cpRepo.GetWeekStats(searchProfile.SearchProfileId, numWeeks, (DayOfWeek)searchProfile.StartDayOfWeek, toDate, false);
            this.ChartObj = new OrdersCPOChart(stats);
        }

        public Attachment GenerateSpreadsheetAttachment()
        {
            Attachment attachment = null;
            GenerateSpreadsheet();
            if (this.Spreadsheet != null)
            {
                attachment = this.Spreadsheet.GetAsAttachment(simpleReport.Attachment_Filename);
            }
            return attachment;
        }
        private void GenerateSpreadsheet()
        {
            string templateFolder = ConfigurationManager.AppSettings["PATH_Search"];
            this.Spreadsheet = DAGenerators.Spreadsheets.Generator.GenerateSearchReport(
                cpRepo,
                templateFolder,
                simpleReport.SearchProfile.SearchProfileId,
                simpleReport.Attachment_NumWeeks,
                simpleReport.Attachment_NumMonths
                );
        }

        public void DisposeResources()
        {
            if (this.ChartObj != null)
                this.ChartObj.DisposeResources();
            if (this.Spreadsheet != null)
                this.Spreadsheet.DisposeResources();
        }
    }
}
