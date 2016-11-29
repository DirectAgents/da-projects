using System;

namespace DirectAgents.Domain.Entities.RevTrack
{
    public class ProgSummary
    {
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }

        public decimal Cost { get; set; }
        // conversions? (postclick & postview?)
    }

    public class ProgExtraItem
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }

        public string Description { get; set; }
        public decimal Cost { get; set; }
        public decimal Revenue { get; set; }
    }
}
