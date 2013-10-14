using System;
using System.Linq;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;

namespace CakeExtracter.Reports
{
    class SearchReport : IReport
    {
        private readonly IClientPortalRepository cpRepo;
        private readonly Advertiser advertiser;

        public SearchReport(IClientPortalRepository cpRepo, Advertiser advertiser)
        {
            this.cpRepo = cpRepo;
            this.advertiser = advertiser;
        }

        public string Generate()
        {
            var toDate = advertiser.AutomatedReportsNextSendAfter.Value;
            var fromDate = toDate.AddDays(-1 * advertiser.AutomatedReportsPeriodDays + 1);
            var stat = this.cpRepo.GetSearchStats(advertiser.AdvertiserId, fromDate, toDate);

            var template = new SearchReportRuntimeTextTemplate();
            template.AdvertiserName = advertiser.AdvertiserName;
            template.Week = stat.Range;
            template.Revenue = stat.Revenue;
            template.Cost = stat.Cost;
            template.ROAS = stat.ROAS;
            template.Margin = stat.Margin;
            template.Orders = stat.Orders;
            template.CPO = stat.CPO;
            string content = template.TransformText();

            return content;
        }
    }
}
