using System.Collections.Generic;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Web.Areas.ProgAdmin.Models
{
    public class SpecialPlatformSummariesVM
    {
        public Platform Platform { get; set; }
        public IEnumerable<SpecialPlatformSummary> SpecialPlatformSummaries { get; set; }

        public SpecialPlatformSummariesVM() { }
    }
}