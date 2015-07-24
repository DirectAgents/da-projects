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

        public Item Item { get; set; }
    }
}