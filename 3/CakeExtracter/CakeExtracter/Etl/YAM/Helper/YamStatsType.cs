namespace CakeExtracter.Etl.YAM.Helper
{
    internal class YamStatsType
    {
        private const string AllArg = "ALL";
        private const string DailyArg = "DAILY";
        private const string CampaignArg = "CAMP";
        private const string LineArg = "LINE";
        private const string AdArg = "AD";
        private const string CreativeArg = "CREAT";
        private const string PixelArg = "PIXEL";

        public bool Daily { get; set; }
        public bool Campaign { get; set; }
        public bool Line { get; set; }
        public bool Ad { get; set; }
        public bool Creative { get; set; }
        public bool Pixel { get; set; }

        public bool All => Daily && Campaign && Line && Ad && Creative && Pixel;

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
    }
}
