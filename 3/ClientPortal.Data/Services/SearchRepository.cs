using ClientPortal.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientPortal.Data.Services
{
    public class SearchRepository
    {
        public IQueryable<SearchStat> GetWeekStats()
        {
            var stats = new List<SearchStat> {
                new SearchStat(true, 5, 12, 378754, 3268, 85, 15295.83M, 3450.53M),
                new SearchStat(true, 5, 19, 395107, 3364, 74, 10818.05M, 3756.82M),
                new SearchStat(true, 5, 26, 431054, 3416, 86, 10812.99M, 3889.59M),
                new SearchStat(true, 6, 2, 363638, 3207, 79, 12775.10M, 3436.27M),
            };
            return stats.AsQueryable();
        }

        public IQueryable<SearchStat> GetMonthStats()
        {
            var stats = new List<SearchStat>
            {
                new SearchStat(false, 3, 31, 2214862, 17262, 382, 49044.51M, 18339.43M),
                new SearchStat(false, 4, 30, 1867419, 16016, 374, 54776.48M, 17673.75M),
                new SearchStat(false, 5, 31, 1712332, 14696, 363, 57189.38M, 16079.54M),
                new SearchStat(false, 6, 3, 198700, 1358, 23, 3205.80M, 1466.40M),
            };
            return stats.AsQueryable();
        }

        public IQueryable<SearchStat> GetChannelStats()
        {
            string google = "Google_AdWords";
            string bing = "Bing_Ads";
            var stats = new List<SearchStat>
            {
                new SearchStat(true, 5, 26, 42609, 3044, 75, 9225.10m, 3586.54m, google),
                new SearchStat(true, 5, 26, 6445, 372, 11, 1587.89m, 303.05m, bing),
                new SearchStat(true, 6, 2, 357143, 2840, 64, 9965.30m, 3151.70m, google),
                new SearchStat(true, 6, 2, 6495, 367, 15, 2809.80m, 284.57m, bing),
            };
            return stats.AsQueryable();
        }

        public IQueryable<SearchStat> GetCampaignStats(string channel)
        {
            List<SearchStat> stats = new List<SearchStat>();
            if (channel == null || channel.Contains("Google"))
                stats.AddRange(new List<SearchStat> {
                    new SearchStat(true, 6, 2, 6025, 118, 4, 339.95m, 121.62m, "DA - \"Apple\" Memory - Keywords"),
                    new SearchStat(true, 6, 2, 6562, 293, 13, 2802.85m, 320.46m, "DA - \"Mac\" Memory - Keywords"),
                });
            if (channel == null || channel.Contains("Bing"))
                stats.AddRange(new List<SearchStat> {
                    new SearchStat(true, 6, 2, 1562, 67, 3, 599.97m, 71.86m, "DA - BING - \"Mac\" Memory - Keywords"),
                    new SearchStat(true, 6, 2, 1224, 116, 4, 419.96m, 74.01m, "DA - BING - iMac Memory"),
                });
            return stats.AsQueryable();
        }
    }
}
