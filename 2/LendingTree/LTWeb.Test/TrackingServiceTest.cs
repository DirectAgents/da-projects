using System;
using LTWeb.Service.Tracking;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LTWeb.Test
{
    [TestClass]
    public class TrackingServiceTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var trackingService = new TrackingService("https://login.directagents.com/api/", "FCjdYAcwQE");
            trackingService.Execute(
                "1/track.asmx/MassConversionInsert",
                new InsertConversionParameters
                    {
                        AffiliateId = 22926,
                        BillingOption = BillingOption.ignore_billing,
                        CampaignId = 8714,
                        ConversionDate = DateTime.Now,
                        CreativeId = 293,
                        Note = "",
                        Payout = 5m,
                        Received = 10m,
                        SubAffiliate = "",
                        TotalToInsert = 1
                    });
        }
    }
}