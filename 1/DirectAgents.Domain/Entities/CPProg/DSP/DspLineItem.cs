namespace DirectAgents.Domain.Entities.CPProg.DSP
{
    /// <summary>Database dsp lineitem entity.</summary>
    public class DspLineItem : DspDbEntity
    {
        public string AdvertiserReportId { get; set; }

        public string AdvertiserName { get; set; }

        public string OrderReportId { get; set; }

        public string OrderName { get; set; }
    }
}
