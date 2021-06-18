using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Web.Areas.ProgAdmin.Models
{
    public class CustomSpecialPlatformSummariesVM
    {
        public Platform Platform { get; set; }

        public IEnumerable<CustomSpecialPlatformLatestsSummary> CustomSpecialPlatformSummaries { get; set; }

        public CustomSpecialPlatformSummariesVM() { }
    }
}