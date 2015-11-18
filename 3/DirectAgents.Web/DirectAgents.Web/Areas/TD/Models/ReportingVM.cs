using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Web.Areas.TD.Models
{
    public class ReportingVM
    {
        public Campaign Campaign { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}