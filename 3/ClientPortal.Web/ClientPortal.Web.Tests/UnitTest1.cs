using System;
using System.IO;
using System.Linq;
using System.Text;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using ClientPortal.Data.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientPortal.Web.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var db = new ClientPortalDWContext())
            {
                var result = db.ClicksByDevice(278, new DateTime(2013, 7, 1), new DateTime(2013, 7, 27))
                               .OrderByDescending(c => c.ClickCount)
                               .ToList();
                var sb = new StringBuilder();
                var tw = new StringWriter(sb);
                var serializer = Newtonsoft.Json.JsonSerializer.Create(new Newtonsoft.Json.JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented
                });
                serializer.Serialize(tw, result);
                Console.WriteLine(sb);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            using (var db = new ClientPortalDWContext())
            {
                var result = db.ConversionsByRegion(278, new DateTime(2013, 7, 1), new DateTime(2013, 7, 27))
                               .ToList();
                Console.WriteLine(result.ToArray().ToJson());

            }
        }

        //[TestMethod]
        //public void Test_ClientPortalRepository_GetCampaignWeekStats()
        //{
        //    using (var context = new ClientPortalContext())
        //    {
        //        var repo = new ClientPortalRepository(context);
        //        var result = repo.GetCampaignWeekStats(90001, new DateTime(2013, 8, 1), new DateTime(2013, 8, 28));
        //        Console.WriteLine(result.ToArray().ToJson());
        //    }
        //}

        [TestMethod]
        public void Test_ClientPortalRespository_GetSearchDailySummaries()
        {
            using (var context = new ClientPortalContext())
            {
                var startDate = new DateTime(2013, 7, 1);
                var endDate = new DateTime(2013, 8, 28);
                var repo = new ClientPortalRepository(context);
                var weeks = CalenderWeek.Generate(startDate, endDate, DayOfWeek.Monday);
                var result = repo
                                .GetSearchDailySummaries(90001, null, startDate, endDate)
                                .Select(c => new
                                {
                                    c.Date,
                                    c.SearchCampaign.Channel,
                                    c.SearchCampaign.SearchCampaignName,
                                    c.Orders,
                                    c.Revenue,
                                    c.Cost
                                })
                                .AsEnumerable()
                                .GroupBy(c => new 
                                {
                                    Week = weeks.First(w => w.EndDate >= c.Date), 
                                    c.Channel, 
                                    c.SearchCampaignName 
                                })
                                .OrderBy(c => c.Key.Week.StartDate)
                                .ThenBy(c => c.Key.SearchCampaignName)
                                .Select(c => new
                                {
                                    c.Key,
                                    TotalOrders = c.Sum(o => o.Orders),
                                    TotalRevenue = c.Sum(r => r.Revenue),
                                    TotalCost = c.Sum(co => co.Cost)
                                })
                                .Select(c => new WeeklySearchStat
                                {
                                    StartDate = c.Key.Week.StartDate,
                                    EndDate = c.Key.Week.EndDate,
                                    Channel = c.Key.Channel,
                                    Campaign = c.Key.SearchCampaignName,
                                    ROAS = c.TotalCost == 0 ? 0 : (int)Math.Round(100 * c.TotalRevenue / c.TotalCost),
                                    CPO = c.TotalOrders == 0 ? 0 : Math.Round(c.TotalCost / c.TotalOrders, 2)
                                });

                Console.WriteLine(result.ToArray().ToJson());
            }
        }

        //[TestMethod]
        //public void TestMethod3()
        //{
        //    using (var db = new ClientPortalContext())
        //    {
        //        var repo = new ClientPortal.Data.Services.ClientPortalRepository(db);
        //        var a = repo.GetCampaignWeekStats(90001, 52, null);
        //        Console.WriteLine(a.ToArray().ToJson());
        //    }
        //}
    }
    static class Ext
    {
        public static string ToJson(this object[] array)
        {
            var sb = new StringBuilder();
            var tw = new StringWriter(sb);
            var serializer = Newtonsoft.Json.JsonSerializer.Create(new Newtonsoft.Json.JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented
            });
            serializer.Serialize(tw, array);
            return sb.ToString();
        }
    }
}
