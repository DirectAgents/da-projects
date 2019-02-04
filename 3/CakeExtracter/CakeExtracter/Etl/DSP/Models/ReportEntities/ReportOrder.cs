namespace CakeExtracter.Etl.DSP.Models
{
    /// <summary>DSp report entity for orders data.</summary>
    internal class ReportOrder: DspReportEntity
    {
        public string AdvertiserReportId { get; set; }

        public string Advertiser { get; set; }
    }
}
