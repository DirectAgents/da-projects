using ClientPortal.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientPortal.Web.Models
{
    public class DashboardModel
    {
        public AdvertiserSummary SummaryMTD { get; set; } // month-to-date
        public AdvertiserSummary SummaryLMTD { get; set; } // last-month-to-date
        public AdvertiserSummary SummaryLM { get; set; } // last-month
    }
}