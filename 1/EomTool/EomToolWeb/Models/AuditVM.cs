using System.Collections.Generic;
using EomTool.Domain.DTOs;
using EomTool.Domain.Entities;

namespace EomToolWeb.Models
{
    public class AuditVM
    {
        public string CurrentEomDateString { get; set; }

        public IEnumerable<AuditSummary> AuditSummaries { get; set; }
        public IEnumerable<Audit> Audits { get; set; }

        public IEnumerable<Advertiser> Advertisers { get; set; }
        public IEnumerable<Campaign> Campaigns { get; set; }
        public IEnumerable<Affiliate> Affiliates { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public Item Item { get; set; }

        public int? AdvId { get; set; }
        public int? AffId { get; set; }
    }
}