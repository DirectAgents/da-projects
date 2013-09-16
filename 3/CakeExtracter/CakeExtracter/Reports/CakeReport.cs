using System;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;

namespace CakeExtracter.Reports
{
    class CakeReport : IReport
    {
        private readonly IClientPortalRepository cpRepo;
        private readonly Advertiser advertiser;

        public CakeReport(IClientPortalRepository cpRepo, Advertiser advertiser)
        {
            this.cpRepo = cpRepo;
            this.advertiser = advertiser;
        }

        public string Generate()
        {
            var toDate = advertiser.AutomatedReportsNextSendAfter.Value;
            var fromDate = toDate.AddDays(advertiser.AutomatedReportsPeriodDays * -1);
            var dateRangeSummary = this.cpRepo.GetDateRangeSummary(fromDate, toDate, advertiser.AdvertiserId, null, advertiser.ShowConversionData);
            if (dateRangeSummary == null)
            {
                var msg = "Cannot generate report for advertiser id {0}, stats not synched?";
                Logger.Warn(msg);
                throw new Exception(msg);
            }

            var template = new CakeReportRuntimeTextTemplate();
            template.AdvertiserName = advertiser.AdvertiserName;
            template.Week = string.Format("{0} - {1}", fromDate.ToShortDateString(), toDate.ToShortDateString());
            template.Clicks = dateRangeSummary.Clicks;
            template.Leads = dateRangeSummary.Conversions;
            template.Rate = dateRangeSummary.ConversionRate;
            template.Spend = dateRangeSummary.Revenue;
            template.Conv = "0";
            template.ConversionValueName = advertiser.ConversionValueName;
            string content = template.TransformText();

            return content;
        }
    }
}
