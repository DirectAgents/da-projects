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
            var stats = this.cpRepo.GetWeekStats(advertiser.AdvertiserId, 2);
            var row = stats.ToList().FirstOrDefault();
            if (row == null)
            {
                var msg = "Cannot generate report for advertiser id {0}, stats not synched?";
                Logger.Warn(msg);
                throw new Exception(msg);
            }

            var template = new SearchReportRuntimeTextTemplate();
            template.AdvertiserName = advertiser.AdvertiserName;
            template.Week = row.Title;
            template.Revenue = row.Revenue;
            template.Cost = row.Cost;
            template.ROAS = row.ROAS;
            template.Margin = row.Margin;
            template.Orders = row.Orders;
            template.CPO = row.CPO;
            string content = template.TransformText();

            return content;
        }
    }
}
