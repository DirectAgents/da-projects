using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ClientPortal.Web.Models.Cake;

namespace CakeExtracter
{
    // TODO: replace Console.WriteLine with Logger.Log

    class Program
    {
        static string apiKey = ConfigurationManager.AppSettings["CakeApiKey"];
        static string baseDir = ConfigurationManager.AppSettings["CacheDirectory"];
        static bool useCache = bool.Parse(ConfigurationManager.AppSettings["UseCache"]);
        static bool useParallel = bool.Parse(ConfigurationManager.AppSettings["UseParallel"]);

        static string usage = @"Usage: 
    CakeExtracter <advertiser-id> <from-date> <to-date> <clicks|conversions|both>    -- synch advertiser for a date range
    CakeExtracter scheduler                                                          -- run the scheduler";

        static Program()
        {
            if (useCache && !Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }
        }

        public static void Main(string[] args)
        {
            Database.SetInitializer<UsersContext>(null);

            if ((args.Length == 1) && (args[0] == "scheduler"))
            {
                Scheduler.Run();
            }
            else if (args.Length == 4)
            {
                Syncher.Run(args);
            }
            else
            {
                Console.WriteLine(usage);
            }
        }

        static class Scheduler
        {
            public static void Run()
            {
                ticker = new Timer(TimerMethod, null, 1000, 1000);

                Console.WriteLine("Press ENTER to stop the scheduler..");
                Console.ReadLine();
            }

            private static Timer ticker;

            public static void TimerMethod(object state)
            {
                Console.Write(".");
            }
        }

        static class Syncher
        {
            public static void Run(string[] args)
            {
                var advertiserId = int.Parse(args[0]);
                var fromDate = DateTime.Parse(args[1]);
                var toDate = DateTime.Parse(args[2]);
                var operation = args[3];

                if (useParallel)
                {
                    Parallel.For(0, (toDate - fromDate).Days, i =>
                    {
                        DoSynch(advertiserId, fromDate, operation, i);
                    });
                }
                else
                {
                    for (int i = 0; i < (toDate - fromDate).Days; i++)
                    {
                        DoSynch(advertiserId, fromDate, operation, i);
                    }
                }
            }

            private static void DoSynch(int advertiserId, DateTime fromDate, string operation, int i)
            {
                var date = fromDate.AddDays(i);

                bool doClicks = operation == "clicks" || operation == "both";
                bool doConversions = operation == "conversions" || operation == "both";

                if (doClicks)
                {
                    var clicks = ExtractClicks(advertiserId, date);
                    DeleteClicks(advertiserId, date);
                    LoadClicks(clicks);
                }

                if (doConversions)
                {
                    var conversions = ExtractConversions(advertiserId, date);
                    DeleteConversions(advertiserId, date);
                    LoadConversions(conversions);
                }
            }

            //
            // Conversions
            //

            private static void DeleteConversions(int advertiserId, DateTime date)
            {
                using (var db = new UsersContext())
                {
                    string deleteSql = "delete from Conversion where advertiser_advertiser_id = {0} and conversion_date between {1} and {2}";
                    int rowCount = db.Database.ExecuteSqlCommand(deleteSql, advertiserId, date, date.AddDays(1));

                    Console.WriteLine("deleted {0} conversions", rowCount);
                }
            }

            private static conversion_report_response ExtractConversions(int advertiserId, DateTime startDate)
            {
                var result = ExtractConversionsFromFile(advertiserId, startDate) ?? ExtractConversionsFromCake(advertiserId, startDate);
                return result;
            }

            private static conversion_report_response ExtractConversionsFromFile(int advertiserId, DateTime startDate)
            {
                string fileName = string.Format(baseDir + "conversions_{0}_{1}.txt", advertiserId, startDate.ToString("MM_dd_yyyy"));

                if (!File.Exists(fileName))
                    return null;

                Console.WriteLine("Extracting conversions from file: {0}..", fileName);

                var serializer = new XmlSerializer(typeof(conversion_report_response));
                var reader = new StreamReader(fileName);
                var result = (conversion_report_response)serializer.Deserialize(reader);

                return result;
            }

            private static conversion_report_response ExtractConversionsFromCake(int advertiserId, DateTime startDate)
            {
                Console.WriteLine("Extracting conversions for advertiser {0} on {1}..", advertiserId, startDate);

                var endDate = startDate.AddDays(1);
                int affiliateId = 0;
                int offerId = 0;
                int campaignId = 0;
                int creativeId = 0;
                bool includeTests = false;
                int startAtRow = 0;
                int rowLimit = 0;
                ConversionsSortFields sortFields = ConversionsSortFields.conversion_date;
                bool isDescending = false;

                var reports = new reports();
                var result = reports.Conversions(apiKey, startDate, endDate, affiliateId, advertiserId, offerId, campaignId, creativeId, includeTests, startAtRow, rowLimit, sortFields, isDescending);

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

            private static void DeleteClicks(int advertiserId, DateTime date)
            {
                using (var db = new UsersContext())
                {
                    string deleteSql = "delete from Click where advertiser_advertiser_id = {0} and click_date between {1} and {2}";
                    int rowCount = db.Database.ExecuteSqlCommand(deleteSql, advertiserId, date, date.AddDays(1));

                    Console.WriteLine("deleted {0} clicks", rowCount);
                }
            }

            private static click_report_response ExtractClicks(int advertiserId, DateTime startDate)
            {
                var result = ExtractClicksFromFile(advertiserId, startDate) ?? ExtractClicksFromCake(advertiserId, startDate);
                return result;
            }

            private static click_report_response ExtractClicksFromFile(int advertiserId, DateTime startDate)
            {
                string fileName = string.Format(baseDir + "clicks_{0}_{1}.txt", advertiserId, startDate.ToString("MM_dd_yyyy"));

                if (!File.Exists(fileName))
                    return null;

                Console.WriteLine("Extracting clicks from file: {0}..", fileName);

                var serializer = new XmlSerializer(typeof(click_report_response));
                var reader = new StreamReader(fileName);
                var result = (click_report_response)serializer.Deserialize(reader);

                return result;
            }

            private static click_report_response ExtractClicksFromCake(int advertiserId, DateTime startDate)
            {
                Console.WriteLine("Extracting clicks for advertiser {0} on {1}..", advertiserId, startDate);

                var reports = new reports();
                var endDate = startDate.AddDays(1);
                int affiliateId = 0;
                int offerId = 0;
                int campaignId = 0;
                int creativeId = 0;
                bool includeTests = false;
                int startAtRow = 0;
                int rowLimit = 0;

                var result = reports.Clicks(apiKey, startDate, endDate, affiliateId, advertiserId, offerId, campaignId, creativeId, includeTests, startAtRow, rowLimit);

                return result;
            }

            private static void LoadClicks(click_report_response result)
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

    static class IEnumerableExtensions
    {
        public static IEnumerable<List<T>> InSetsOf<T>(this IEnumerable<T> source, int max)
        {
            var set = new List<T>(max);
            foreach (var item in source)
            {
                set.Add(item);
                if (set.Count == max)
                {
                    yield return set;
                    set = new List<T>(max);
                }
            }
            if (set.Any())
                yield return set;
        }

        public static IEnumerable<List<T>> InSetsOf<T>(this T[] source, int max)
        {
            return InSetsOf(source.AsEnumerable(), max);
        }
    }

    // HACK: this is in two places right now because referencing the web project causes a runtime error..
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<conversion> Conversions { get; set; }
        public DbSet<click> Clicks { get; set; }
    }
}
