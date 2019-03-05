namespace CakeExtracter.Etl.DSP.Models
{
    /// <summary>Dsp report entity for line items data.</summary>
    internal class ReportLineItem : DspReportEntity
    {
        public string AdvertiserReportId { get; set; }

        public string Advertiser { get; set; }

        public string OrderReportId { get; set; }

        public string Order { get; set; }
    }
}
