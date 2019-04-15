using System;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Domain.DTO
{
    public class SpecialPlatformSummary
    {
        public int? AccountId { get; set; }
        public ExtAccount Account { get; set; }
        public DateTime? EarliestDate { get; set; }
        public DateTime? LatestDate { get; set; }
    }
}
