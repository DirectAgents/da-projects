using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ClientPortal.Web.Models;
using ClientPortal.Web.Models.Cake;

namespace CakeExtracter
{
    class Program
    {
        static string apiKey = ConfigurationManager.AppSettings["CakeApiKey"];
        static string baseDir = ConfigurationManager.AppSettings["CacheDirectory"];
        static bool useCache = bool.Parse(ConfigurationManager.AppSettings["UseCache"]);

        static Program()
        {
            if (useCache && !Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }
        }

        public static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Usage: CakeExtracter <advertiser-id> <from-date> <to-date> <clicks|conversions|both>");
                return;
            }

            var advertiserId = int.Parse(args[0]);
            var fromDate = DateTime.Parse(args[1]);
            var toDate = DateTime.Parse(args[2]);
            var operation = args[3];

            Parallel.For(0, (toDate - fromDate).Days, i =>
            {
                var date = fromDate.AddDays(i);

                bool doClicks = operation == "clicks" || operation == "both";
                bool doConversions = operation == "conversions" || operation == "both";
                if (doClicks)
                {
                    var clicks = ExtractClicks(advertiserId, date);
                    LoadClicks(clicks);
                }
                if (doConversions)
                {
                    var conversions = ExtractConversions(advertiserId, date);
                    LoadConversions(conversions);
                }
            });

            Console.Write("Press a key . . . ");
            Console.ReadKey(true);
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

            using (var db = new UsersContext())
            {
                foreach (var item in conversionsResponse.conversions)
                {
                    db.Conversions.Add(item);
                }

                Console.WriteLine("saving..");
                db.SaveChanges();
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

            using (var db = new UsersContext())
            {
                foreach (var item in result.clicks)
                {
                    db.Clicks.Add(item);
                }

                Console.WriteLine("saving..");
                db.SaveChanges();
            }
        }
    }
}
