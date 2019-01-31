namespace CakeExtracter.Etl.DSP.Models
{
    internal class ReportLineItem : DspReportEntity
    {
        public string AdvertiserReportId { get; set; }

        public string Advertiser { get; set; }

        public string OrderReportId { get; set; }

        public string Order { get; set; }
    }
}
