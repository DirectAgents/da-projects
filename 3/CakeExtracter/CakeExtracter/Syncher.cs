using System;
using System.Collections.Generic;
using System.Linq;
using ClientPortal.Data.Contexts;
using ClientPortal.Web.Models.Cake;
using EntityFramework.Extensions;

namespace CakeExtracter
{
    class Syncher
    {
        public void Run(string[] args)
        {
            var advertiserId = int.Parse(args[0]);
            var fromDate = DateTime.Parse(args[1]);
            var toDate = DateTime.Parse(args[2]);

            InitializeMetrics();

            for (int addDays = 0; addDays < (toDate - fromDate).Days; addDays++)
            {
                DoSynch(advertiserId, fromDate, addDays);
            }
        }

        /// <summary>
        /// Ensure the Metrics exists
        /// </summary>
        private static void InitializeMetrics()
        {
            using (var db = new ClientPortalContext())
            {
                if (!db.Metrics.Any(m => m.name == MetricNames.Device))
                {
                    var metric = new Metric { name = MetricNames.Device };
                    db.Metrics.Add(metric);
                }
                if (!db.Metrics.Any(m => m.name == MetricNames.Region))
                {
                    var metric = new Metric { name = MetricNames.Region };
                    db.Metrics.Add(metric);
                }
                db.SaveChanges();
            }
        }

        private void DoSynch(int advertiserId, DateTime fromDate, int addDays)
        {
            var date = fromDate.AddDays(addDays);

            // delete the metric counts for the day we are refreshing
            DeleteMetricCounts(advertiserId, date, null);

            // bring all clicks for a single day into memory
            click_report_response clicks = ExtractClicks(advertiserId, date);

            // DeleteClicks(advertiserId, date);
            // LoadClicks(clicks);

            LoadDeviceCounts(clicks);

            conversion_report_response conversions = ExtractConversions(advertiserId, date);
            DeleteConversions(advertiserId, date);
            LoadConversions(conversions);

            LoadRegionCounts(clicks, conversions);
        }

        //
        // MetricCounts
        //

        private static void DeleteMetricCounts(int advertiserId, DateTime date, bool? conversionsOnly)
        {
            var datePlusOne = date.AddDays(1);

            using (var db = new ClientPortalContext())
            {
                var offerIds = db.Offers.Where(o => o.Advertiser_Id == advertiserId).Select(o => o.Offer_Id).ToList();

                var metricCounts = db.MetricCounts.Where(mc => offerIds.Contains(mc.offer_id) &&
                                                               mc.date >= date &&
                                                               mc.date < datePlusOne);
                if (conversionsOnly.HasValue)
                    metricCounts = metricCounts.Where(mc => mc.conversions_only == conversionsOnly.Value);

                int numDeleted = metricCounts.Delete();

                db.SaveChanges();

                Console.WriteLine("deleted {0} MetricCounts", numDeleted);
            }
        }

        private void LoadRegionCounts(click_report_response clickReport, conversion_report_response conversionReport)
        {
            if (conversionReport.conversions.Length > 0)
            {
                using (var db = new ClientPortalContext())
                {
                    var conversion0 = conversionReport.conversions[0];
                    var offerId = conversion0.offer.offer_id;
                    var date = new DateTime(conversion0.conversion_date.Year, conversion0.conversion_date.Month,
                                            conversion0.conversion_date.Day);

                    // Match converions to clicks
                    var conversionsAndClicksQuery = from cv in conversionReport.conversions
                                                    from ck in clickReport.clicks
                                                                          .Where(c => c.click_id == cv.click_id)
                                                                          .DefaultIfEmpty()
                                                    select new {Conversion = cv, Click = ck};

                    var conversionsAndClicks = conversionsAndClicksQuery
                        .ToArray();

                    // Find unmatched
                    var unmatched = conversionsAndClicks
                        .Where(c => c.Click == null)
                        .ToArray();

                    var conversionsNeedingClick = unmatched.Select(c => c.Conversion);

                    // Extract clicks to fix unmatched
                    var extractedClicks = ClicksByConversion(conversionsNeedingClick)
                        .SelectMany(c => c.clicks)
                        .ToArray();

                    // Fix unmatched
                    var fixedUp = unmatched.Select(c => new
                        {
                            c.Conversion,
                            Click = extractedClicks.FirstOrDefault(extracted => extracted.click_id == c.Conversion.click_id)
                        })
                        .ToArray();

                    // Find matched
                    var matched = conversionsAndClicks.Where(c => c.Click != null);

                    // Combine matched and fixed
                    var conversions = new[] { matched, fixedUp }.SelectMany(c => c).ToArray();

                    // Group by country code and region code
                    var groups = conversions.Where(c => c.Click != null)
                        .GroupBy(c => new { Country = c.Click.country.country_code, Region = c.Click.region.region_code })
                        .ToArray();

                    // Check the count
                    int totalConversions = conversionReport.conversions.Count();
                    int countedMetrics = groups.Sum(c => c.Count());
                    int uncountedMetrics = totalConversions - countedMetrics;
                    if (uncountedMetrics == 0)
                        Console.WriteLine("Counts Match!");
                    else
                        Console.WriteLine("Counts are off: {0} - {1} = {2}", totalConversions, countedMetrics, uncountedMetrics);

                    // Get the metric entity for Region
                    var metric = db.Metrics.Single(m => m.name == MetricNames.Region);

                    // Ensure that a MetricValue for each group exists
                    foreach (var regionGroup in groups)
                    {
                        var existingMetric = metric.MetricValues.FirstOrDefault(c => c.name == regionGroup.Key.Region && c.code == regionGroup.Key.Country);

                        if (existingMetric == null)
                        {
                            Console.WriteLine("adding new MetricValue: {0}", regionGroup.Key.Region + "," + regionGroup.Key.Country);

                            var metricValue = new MetricValue()
                            {
                                name = regionGroup.Key.Region,
                                code = regionGroup.Key.Country
                            };

                            metric.MetricValues.Add(metricValue);
                        }

                        db.SaveChanges();
                    }

                    Console.WriteLine("adding MetricCounts..");

                    // Add a MetricCount for each region
                    foreach (var regionGroup in groups)
                    {
                        var metricValue = metric.MetricValues.First(c => c.name == regionGroup.Key.Region && c.code == regionGroup.Key.Country);

                        var offerGroups = regionGroup.GroupBy(d => d.Conversion.offer.offer_id);

                        foreach (var og in offerGroups)
                        {
                            var metricCount = new MetricCount()
                            {
                                offer_id = og.Key,
                                date = date,
                                conversions_only = false,
                                count = og.Count()
                            };

                            metricValue.MetricCounts.Add(metricCount);
                        }

                        db.SaveChanges();
                    }
                }
            }
        }

        private static IEnumerable<click_report_response> ClicksByConversion(IEnumerable<conversion> conversions)
        {
            var results =
                conversions
                    .GroupBy(c => new
                        {
                            c.affiliate.affiliate_id,
                            c.offer.offer_id,
                            c.campaign_id,
                            c.creative.creative_id,
                            c.click_date.Value.Date,
                            c.advertiser.advertiser_id
                        })
                    .Select(g =>
                        {
                            var reports = new reports();
                            int affiliateId = g.Key.affiliate_id;
                            int offerId = g.Key.offer_id;
                            int campaignId = g.Key.campaign_id;
                            int advertiserId = g.Key.advertiser_id;
                            int creativeId = g.Key.creative_id;
                            const bool includeTests = false;
                            const int startAtRow = 0;
                            const int rowLimit = 0;
                            var clickDate = g.Key.Date;
                            var endDate = clickDate.AddDays(1);

                            Console.WriteLine("Extracting clicks: affiliate_id={0},offer_id={1},campaign_id={2},creative_id={3},click_date={4},advertiser_id={5}",
                                affiliateId, offerId, campaignId, creativeId, clickDate, advertiserId);

                            var result = reports.Clicks(
                                Globals.ApiKey, clickDate, endDate, affiliateId, advertiserId,
                                offerId, campaignId, creativeId, includeTests, startAtRow, rowLimit);

                            return result;
                        });
            return results;
        }

        private static void LoadDeviceCounts(click_report_response clickReport)
        {
            if (clickReport.clicks.Length > 0)
            {
                using (var db = new ClientPortalContext())
                {
                    var deviceMetric = db.Metrics.Where(m => m.name == MetricNames.Device).First();

                    var click0 = clickReport.clicks[0];
                    var offerId = click0.offer.offer_id;
                    var date = new DateTime(click0.click_date.Year, click0.click_date.Month, click0.click_date.Day);

                    var deviceGroups = clickReport.clicks.GroupBy(c => c.device.device_name);

                    // Ensure that all MetricValues exists
                    foreach (var dg in deviceGroups)
                    {
                        if (!deviceMetric.MetricValues.Any(mv => mv.name == dg.Key))
                        {
                            Console.WriteLine("adding new MetricValue: {0}", dg.Key);

                            var metricValue = new MetricValue()
                                {
                                    name = dg.Key,
                                    code = dg.First().device.device_id.ToString()
                                };
                            deviceMetric.MetricValues.Add(metricValue);
                        }
                    }

                    db.SaveChanges();

                    Console.WriteLine("adding MetricCounts..");

                    // Add MetricCounts
                    foreach (var dg in deviceGroups)
                    {
                        var metricValue = db.MetricValues.First(mv => mv.name == dg.Key);
                        var offerGroups = dg.GroupBy(d => d.offer.offer_id);

                        foreach (var og in offerGroups)
                        {
                            var metricCount = new MetricCount()
                                {
                                    offer_id = og.Key,
                                    date = date,
                                    conversions_only = false,
                                    count = og.Count()
                                };
                            metricValue.MetricCounts.Add(metricCount);
                        }

                        db.SaveChanges();
                    }
                }
            }
        }

        //
        // Conversions
        //

        private void DeleteConversions(int advertiserId, DateTime date)
        {
            using (var db = new UsersContext())
            {
                const string deleteSql = "delete from Conversion where advertiser_advertiser_id = {0} and conversion_date between {1} and {2}";
                int rowCount = db.Database.ExecuteSqlCommand(deleteSql, advertiserId, date, date.AddDays(1));
                Console.WriteLine("deleted {0} conversions", rowCount);
            }
        }

        private static conversion_report_response ExtractConversions(int advertiserId, DateTime startDate)
        {
            Console.WriteLine("Extracting conversions for advertiser {0} on {1}..", advertiserId, startDate);

            var endDate = startDate.AddDays(1);
            const int affiliateId = 0;
            const int offerId = 0;
            const int campaignId = 0;
            const int creativeId = 0;
            const bool includeTests = false;
            const int startAtRow = 0;
            const int rowLimit = 0;
            const ConversionsSortFields sortFields = ConversionsSortFields.conversion_date;
            const bool isDescending = false;

            var reports = new reports();

            var result = reports.Conversions(
                Globals.ApiKey, startDate, endDate, affiliateId, advertiserId, offerId,
                campaignId, creativeId, includeTests, startAtRow, rowLimit, sortFields, isDescending);

            return result;
        }

        private static void LoadConversions(conversion_report_response conversionsResponse)
        {
            Console.WriteLine("loading {0} conversions..", conversionsResponse.row_count);

            int total = conversionsResponse.row_count;
            int count = 0;

            using (var db = new UsersContext())
            {
                foreach (var set in conversionsResponse.conversions.InSetsOf(2000))
                {
                    count += set.Count;

                    Console.WriteLine("saving {0}/{1} conversions..", count, total);

                    set.ForEach(c => db.Conversions.Add(c));
                    db.SaveChanges();
                }
            }
        }

        //
        // Clicks
        //

        //private void DeleteClicks(int advertiserId, DateTime date)
        //{
        //    var datePlusOne = date.AddDays(1);
        //    using (var db = new UsersContext())
        //    {
        //        string deleteSql = "delete from Click where advertiser_advertiser_id = {0} and click_date between {1} and {2}";
        //        int rowCount = db.Database.ExecuteSqlCommand(deleteSql, advertiserId, date, datePlusOne);
        //        Console.WriteLine("deleted {0} clicks", rowCount);
        //    }
        //}

        private static click_report_response ExtractClicks(int advertiserId, DateTime startDate)
        {
            Console.WriteLine("Extracting clicks for advertiser {0} on {1}..", advertiserId, startDate);

            var reports = new reports();
            var endDate = startDate.AddDays(1);
            const int affiliateId = 0;
            const int offerId = 0;
            const int campaignId = 0;
            const int creativeId = 0;
            const bool includeTests = false;
            const int startAtRow = 0;
            const int rowLimit = 0;

            var result = reports.Clicks(
                Globals.ApiKey, startDate, endDate, affiliateId, advertiserId, offerId,
                campaignId, creativeId, includeTests, startAtRow, rowLimit);

            return result;
        }

        private void LoadClicks(click_report_response result)
        {
            Console.WriteLine("loading {0} clicks..", result.row_count);
            int total = result.row_count;
            int count = 0;
            foreach (var set in result.clicks.InSetsOf(2000))
            {
                count += set.Count;
                Console.WriteLine("saving {0}/{1} clicks..", count, total);
                using (var db = new UsersContext())
                {
                    set.ForEach(c => db.Clicks.Add(c));
                    db.SaveChanges();
                }
            }
        }
    }
}