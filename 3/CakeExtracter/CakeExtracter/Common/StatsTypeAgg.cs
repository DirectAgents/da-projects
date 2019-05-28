namespace CakeExtracter.Commands
{
    public class StatsTypeAgg
    {
        public const string AllArg = "ALL";
        public const string DailyArg = "DAILY";
        public const string StrategyArg = "STRAT";
        public const string AdSetArg = "ADSET";
        public const string CreativeArg = "CREAT";
        public const string SiteArg = "Site";
        public const string ConversionArg = "CONV";
        public const string KeywordArg = "KEY";
        public const string SearchTermArg = "STERM";

        public bool Daily { get; set; }

        public bool Strategy { get; set; }

        public bool AdSet { get; set; }

        public bool Creative { get; set; }

        public bool Site { get; set; }

        public bool Conv { get; set; }

        public bool Keyword { get; set; }

        public bool SearchTerm { get; set; }

        public bool All => Daily && Strategy && AdSet && Creative && Site && Conv && Keyword && SearchTerm;

        public StatsTypeAgg()
        {
        }

        public StatsTypeAgg(string statsTypeString)
        {
            var statsTypeUpper = (statsTypeString == null) ? "" : statsTypeString.ToUpper();
            if (string.IsNullOrWhiteSpace(statsTypeUpper) || statsTypeUpper == AllArg)
            {
                SetAllTrue();
            }
            else if (statsTypeUpper.StartsWith(DailyArg))
            {
                Daily = true;
            }
            else if (statsTypeUpper.StartsWith(StrategyArg))
            {
                Strategy = true;
            }
            else if (statsTypeUpper.StartsWith(AdSetArg))
            {
                AdSet = true;
            }
            else if (statsTypeUpper.StartsWith(CreativeArg))
            {
                Creative = true;
            }
            else if (statsTypeUpper.StartsWith(SiteArg))
            {
                Site = true;
            }
            else if (statsTypeUpper.StartsWith(ConversionArg))
            {
                Conv = true;
            }
            else if (statsTypeUpper.StartsWith(KeywordArg))
            {
                Keyword = true;
            }
            else if (statsTypeUpper.StartsWith(SearchTermArg))
            {
                SearchTerm = true;
            }
        }

        public void SetAllTrue()
        {
            Daily = true;
            Strategy = true;
            AdSet = true;
            Creative = true;
            Site = true;
            Conv = true;
            Keyword = true;
            SearchTerm = true;
        }

        public string GetStatsTypeString()
        {
            return Daily
                ? DailyArg
                : Strategy
                    ? StrategyArg
                    : AdSet
                        ? AdSetArg
                        : Creative
                            ? CreativeArg
                            : Site
                                ? SiteArg
                                : Conv
                                    ? ConversionArg
                                    : Keyword
                                        ? KeywordArg
                                        : SearchTerm
                                            ? SearchTermArg
                                            : AllArg;
        }
    }
}