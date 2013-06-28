using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CakeExtracter.CakeMarketingApi;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;
using CakeExtracter.Etl;
using CakeExtracter.Etl.CakeMarketing;
using CakeExtracter.Etl.CakeMarketing.Entities;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;

namespace CakeExtracter.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
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
        public void Etl_DailySummariesExtracter()
        {
            var dateRange = new DateRange(new DateTime(2013, 6, 1), new DateTime(2013, 6, 3));
            var extracter = new DailySummariesExtracter(dateRange, 278);
            var loader = new MockLoader();
            var extracterThread = extracter.BeginExtracting();
            var loaderThread = loader.BeginLoading(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        public class MockLoader : Loader<OfferDailySummary>
        {
            protected override int LoadItems(List<OfferDailySummary> items)
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
    }

    public static class Ext
    {
        public static string ToXml<T>(this T[] arr)
        {
            var serializer = new XmlSerializer(typeof (T[]));
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, arr); 
            }
            var result = sb.ToString();
            return result;
        }
    }
}
