namespace DirectAgents.Domain.Entities.CPProg.DSP
{
    public class DspLineItem : DspDbEntity
    {
        public string AdvertiserReportId { get; set; }

        public string AdvertiserName { get; set; }

        public string OrderReportId { get; set; }

        public string OrderName { get; set; }
    }
}
