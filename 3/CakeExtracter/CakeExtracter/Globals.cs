using System.Configuration;

namespace CakeExtracter
{
    public static class Globals
    {
        public static readonly string ApiKey = ConfigurationManager.AppSettings["CakeApiKey"];

        public const string Usage = @"Usage: 
    CakeExtracter <advertiser-id> <from-date> <to-date>  -- synch advertiser for a date range
       -- OR --
    CakeExtracter scheduler                              -- run the scheduler";
    }
}