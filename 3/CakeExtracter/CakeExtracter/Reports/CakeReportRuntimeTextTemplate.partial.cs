namespace CakeExtracter.Reports
{
    public partial class CakeReportRuntimeTextTemplate : CakeReportRuntimeTextTemplateBase
    {
        public string AdvertiserName { get; set; }

        public string Week { get; set; }

        public int? Clicks { get; set; }

        public int? Leads { get; set; }

        public double? Rate { get; set; }

        public decimal? Spend { get; set; }

        public string Conv { get; set; }

        public string ConversionValueName { get; set; }
    }
}
