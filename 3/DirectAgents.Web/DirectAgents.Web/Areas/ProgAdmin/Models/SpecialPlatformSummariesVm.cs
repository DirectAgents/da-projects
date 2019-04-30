using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Web.Areas.ProgAdmin.Models
{
    public class SpecialPlatformSummariesVM
    {
        public Platform Platform { get; set; }
        public IEnumerable<SpecialPlatformLatestsSummary> SpecialPlatformSummaries { get; set; }

        public SpecialPlatformSummariesVM() { }
    }
}