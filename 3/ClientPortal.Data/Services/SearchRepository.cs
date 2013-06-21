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
            var stats = new List<SearchStat>
            {
                new SearchStat(true, 6, 2, 357143, 2840, 64, 9965.30M, 3151.70M, "Google_AdWords"),
                new SearchStat(true, 6, 2, 6495, 367, 15, 2809.80M, 284.57M, "MSN_AdCenter")
            };
            return stats.AsQueryable();
        }
    }
}
