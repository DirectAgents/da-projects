using System;
using System.Linq;
using System.Collections.Generic;
using ClientPortal.Web.Models;
using DirectAgents.Domain.Entities.TD;

namespace ClientPortal.Web.Areas.Prog.Models
{
    public class ReportVM
    {
        public UserInfo UserInfo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public IEnumerable<BasicStat> Stats { get; set; }
        public IEnumerable<LeadInfo> LeadInfos { get; set; }

        public bool AnyUrls()
        {
            return (Stats != null) && Stats.Any(s => !String.IsNullOrWhiteSpace(s.Url));
        }
    }
}