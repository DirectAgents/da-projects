using System;
using System.Collections.Generic;
using System.Globalization;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Web.Areas.ProgAdmin.Models
{
    public class DashboardVM
    {
        public DateTime Month { set { MonthString = value.ToString("d", CultureInfo.InvariantCulture); } }
        public string MonthString { get; set; }

        public IEnumerable<Campaign> Campaigns { get; set; }
        public bool ShowAll { get; set; }
    }
}