using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.CakeMarketingApi;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;
using CakeExtracter.Etl;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.Loaders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BingAds;

namespace CakeExtracter.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void BingAdsTest_Campaigns()
        {
            var bingTest = new BingAds.Test();
            var campaigns = bingTest.GetCampaigns(234647, 886985);
            foreach (var campaign in campaigns)
            {
                Console.WriteLine(campaign.Id + " " + campaign.Name);
            }
        }

        [TestMethod]
        public void BingAdsReport_KeywordPerformance()
        {
            var bingReports = new BingAds.Reports();
            bingReports.GetKeywordPerformance(886985, 51468225);
        }

        [TestMethod]
        public void BingAdsReport_DailySums()
        {
            var bingReports = new BingAds.Reports();
            var startDate = new DateTime(2013, 6, 1);
            var endDate = new DateTime(2013, 6, 30);
            var filepath = bingReports.GetDailySummaries(886985, startDate, endDate);
            Console.WriteLine("Filepath: " + filepath);
        }

        [TestMethod]
        public void CakeMarketingUtility_Advertisers()
        {
            var offers = CakeMarketingUtility.Advertisers();
            Console.WriteLine(offers.ToArray().ToXml());
        }

        [TestMethod]
        public void CakeMarketingUtility_OfferIds()
        {
            const int advertiserId = 278;
            var offerIds = CakeMarketingUtility.OfferIds(advertiserId);
            foreach (var offerId in offerIds)
            {
                Console.WriteLine(offerId);
            }
        }

        [TestMethod]
        public void CakeMarketingUtility_Offers()
        {
            var offers = CakeMarketingUtility.Offers();
            Console.WriteLine(offers.ToArray().ToXml());
        }


        [TestMethod]
        public void Integration_Offers_Extracter_And_Loader()
        {
            var extracter = new OffersExtracter();
            var loader = new OffersLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        [TestMethod]
        public void CakeMarketingUtility_Clicks()
        {
            const int advertiserId = 278;
            const int offerId = 0;
            var dateRange = new DateRange(new DateTime(2013, 6, 1), new DateTime(2013, 6, 2));
            int rowCount;
            var result = CakeMarketingUtility.Clicks(dateRange, advertiserId, offerId, out rowCount);
            Console.WriteLine("ROW COUNT: {0}", rowCount);
            Console.WriteLine(result.ToArray().ToXml());
            Assert.AreEqual(result.Count, rowCount);
            Assert.IsTrue(rowCount > 0, "rowcount must be greater than 0");
        }

        [TestMethod]
        public void Integration_Clicks_Extracter_And_Loader()
        {
            var dateRange = new DateRange(new DateTime(2013, 6, 1), new DateTime(2013, 6, 3));
            var extracter = new ClicksExtracter(dateRange, 278);
            var loader = new ClicksLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        [TestMethod]
        public void CakeMarketingUtility_Conversions()
        {
            const int advertiserId = 278;
            const int offerId = 0;
            var dateRange = new DateRange(new DateTime(2013, 6, 1), new DateTime(2013, 6, 2));
            var result = CakeMarketingUtility.Conversions(dateRange, advertiserId, offerId);
            Console.WriteLine(result.ToArray().ToXml());
        }

        [TestMethod]
        public void CakeMarketingUtility_Traffic()
        {
            var dateRange = new DateRange(new DateTime(2013, 6, 1), new DateTime(2013, 6, 2));
            var result = CakeMarketingUtility.Traffic(dateRange);
            Console.WriteLine(result.ToArray().ToXml());
        }

        [TestMethod]
        public void Etl_DailySummariesExtracter()
        {
            var dateRange = new DateRange(new DateTime(2013, 6, 1), new DateTime(2013, 6, 3));
            var extracter = new DailySummariesExtracter(dateRange, 278);
            var loader = new MockLoader<OfferDailySummary>();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        public class MockLoader<T> : Loader<T>
        {
            protected override int Load(List<T> items)
            {
                var loadedCount = 0;
                if (items != null)
                {
                    Console.WriteLine("Loading {0} items..", items.Count());
                    Console.WriteLine(items.ToArray().ToXml());
                    loadedCount = items.Count;
                }
                return loadedCount;
            }
        }

        [TestMethod]
        public void CakeExtracter_SynchCakeTrafficCommand()
        {
            var obj = new CakeExtracter.Commands.SynchCakeTrafficCommand();
            obj.StartDate = new DateTime(2013, 6, 1);
            obj.EndDate = new DateTime(2013, 7, 1);
            obj.Execute(null);
        }
    }
}
