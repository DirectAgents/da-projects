using ClientPortal.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientPortal.Web.Models
{
    public class DashboardModel
    {
        public List<DateRangeSummary> DateRangeSummaries { get; set; }
    }
}