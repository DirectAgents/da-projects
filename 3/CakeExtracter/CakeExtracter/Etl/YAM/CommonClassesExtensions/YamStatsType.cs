namespace CakeExtracter.Etl.YAM.CommonClassesExtensions
{
    internal class YamStatsType
    {
        public const string AllArg = "ALL";
        public const string DailyArg = "DAILY";
        public const string CampaignArg = "CAMP";
        public const string LineArg = "LINE";
        public const string AdArg = "AD";
        public const string CreativeArg = "CREAT";
        public const string PixelArg = "PIXEL";

        public bool Daily { get; set; }
        public bool Campaign { get; set; }
        public bool Line { get; set; }
        public bool Ad { get; set; }
        public bool Creative { get; set; }
        public bool Pixel { get; set; }

        public bool All => Daily && Campaign && Line && Ad && Creative && Pixel;

        public YamStatsType()
        { }

        public YamStatsType(string statsTypeString)
        {
            var statsTypeUpper = statsTypeString?.ToUpper();
            if (string.IsNullOrEmpty(statsTypeUpper) || string.IsNullOrWhiteSpace(statsTypeUpper) || statsTypeUpper == AllArg)
            {
                SetAllTrue();
            }
            else
            {
                Daily = statsTypeUpper.StartsWith(DailyArg);
                Campaign = statsTypeUpper.StartsWith(CampaignArg);
                Line = statsTypeUpper.StartsWith(LineArg);
                Ad = statsTypeUpper.StartsWith(AdArg);
                Creative = statsTypeUpper.StartsWith(CreativeArg);
                Pixel = statsTypeUpper.StartsWith(PixelArg);
            }
        }

        public void SetAllTrue()
        {
            Daily = Campaign = Line = Ad = Creative = Pixel = true;
        }

        public string GetStatsTypeString()
        {
            return Pixel
                ? PixelArg
                : Ad
                    ? AdArg
                    : Line
                        ? LineArg
                        : Campaign
                            ? CampaignArg
                            : Creative
                                ? CreativeArg
                                : Daily
                                    ? DailyArg
                                    : AllArg;
        }
    }
}
