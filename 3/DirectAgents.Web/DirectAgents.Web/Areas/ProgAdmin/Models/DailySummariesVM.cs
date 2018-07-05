using System;
using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Web.Areas.ProgAdmin.Models
{
    public class DailySummariesVM
    {
        public ExtAccount ExtAccount { get; set; }

        public DateTime? Start { get; set; }
        public string StartString {
            get { return Start.HasValue ? Start.Value.ToShortDateString() : "(start)"; }
        }
        public DateTime? End { get; set; }
        public string EndString {
            get { return End.HasValue ? End.Value.ToShortDateString() : "(end)"; }
        }
        public bool CustomDates { get; set; }

        public IEnumerable<DailySummary> DailySummaries { get; set; }
    }
}