using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EomTool.Domain.Entities
{
    public partial class UnitType
    {
        public const int Analytics = 24;
        public const int AmazonPM = 31;
        public const int AmazonSEO = 32;
        public const int CPA = 4;
        public const int CPC = 8;
        public const int PPC = 15;
        public const int SEO = 19;
        public const int SocialMedia = 22;
        public const int Strategy = 30;
        public const int Sale = 13;
        public const int TradingDesk = 18;
        //etc...

        [NotMapped]
        public static List<int> NonPerformanceMarketing
        {
            get { return (new int[] { Analytics, AmazonPM, AmazonSEO, PPC, SEO, TradingDesk, SocialMedia, Strategy, Sale }).ToList(); }
        }

        public static string ToItemCode(string unitTypeName)
        {
            if (unitTypeName == null)
                return null;
            string unitTypeNameLowered = unitTypeName.ToLower();

            if (unitTypeNameLowered.Contains("affiliate program"))
                return "Affiliate Management";
            switch (unitTypeNameLowered)
            {
                case "call tracking":
                    return "Search";
                case "gsp":
                    return "GSP";
                case "opm":
                    return "Affiiate Program Management";
                case "ppc":
                    return "Search Marketing";
                case "revshare":
                    return "CPA Fee";
                case "seo":
                    return "Search";
                case "trading desk":
                    return "Trading Desk";
                default:
                    if (unitTypeNameLowered.Contains(" fee"))
                        return unitTypeName;
                    else
                        return unitTypeName + " Fee";
            }
        }
    }
}
