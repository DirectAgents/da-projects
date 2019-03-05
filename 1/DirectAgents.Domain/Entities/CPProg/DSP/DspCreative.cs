namespace DirectAgents.Domain.Entities.CPProg.DSP
{
    /// <summary>Database dsp creative entity</summary>
    public class DspCreative : DspDbEntity
    {
        public string AdvertiserReportId { get; set; }

        public string AdvertiserName { get; set; }

        public string OrderReportId { get; set; }

        public string OrderName { get; set; }

        public string LineItemReportId { get; set; }

        public string LineItemName { get; set; }
    }
}
