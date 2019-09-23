using System;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.Models
{
    /// <summary>
    /// Latests data for platform accounts.
    /// </summary>
    public class SpecialPlatformLatestsSummary
    {
        public int? AccountId { get; set; }

        public ExtAccount Account { get; set; }

        public DateTime? EarliestDate { get; set; }

        public DateTime? LatestDate { get; set; }
    }
}
