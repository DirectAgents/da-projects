using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using System;
using System.Linq;

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
    }
}
