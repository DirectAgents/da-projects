namespace CakeExtracter.Etl.DSP.Models
{
    internal class ReportCreative : DspReportEntity
    {
        public string AdvertiserReportId { get; set; }

        public string Advertiser { get; set; }

        public string OrderReportId { get; set; }

        public string Order { get; set; }

        public string LineItemReportId { get; set; }

        public string LineItem { get; set; }
    }
}
