using System;
using CakeMarketing;

namespace CakeExtracter
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: CakeExtracter <advertiser-id> <from-date> <to-date> <clicks|conversions|both>");
                return;
            }
            int advertiserId = int.Parse(args[0]);
            var fromDate = DateTime.Parse(args[1]);
            var toDate = DateTime.Parse(args[2]);
            var operation = args[3];
            if (operation == "clicks")
            {
            }
            else if (operation == "conversions")
            {
            }
            else if (operation == "both")
            {
            }
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }

        static string apiKey = "FCjdYAcwQE";

        public static void ExtractConversions(int advertiserId, DateTime startDate)
        {
            Console.WriteLine("Extracting conversions for advertiser {0} on {1}..", advertiserId, startDate);
            var reports = new reports();
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
            var result = reports.Conversions(apiKey, startDate, endDate, affiliateId,
                                             advertiserId, offerId, campaignId, creativeId,
                                             includeTests, startAtRow, rowLimit,
                                             sortFields, isDescending);
            Console.WriteLine("row count = {0}", result.row_count);
            using (var db = new CakeExtracterContext())
            {
                foreach (var item in result.conversions)
                {
                    db.Conversions.Add(item);
                }
                Console.WriteLine("saving..");
                db.SaveChanges();
            }
        }

        public static void ExtractClicks(int advertiserId, DateTime startDate)
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
            var result = reports.Clicks(apiKey, startDate, endDate, affiliateId, advertiserId, offerId,
                                        campaignId, creativeId, includeTests, startAtRow, rowLimit);
            Console.WriteLine("row count = {0}", result.row_count);
            using (var db = new CakeExtracterContext())
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
