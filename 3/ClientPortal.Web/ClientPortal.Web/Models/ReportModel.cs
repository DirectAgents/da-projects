using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientPortal.Web.Models
{
    public class ReportModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public bool ShowConVal { get; set; }
        public string ConValName { get; set; }
        public bool ConValIsNum { get; set; }
    }
}