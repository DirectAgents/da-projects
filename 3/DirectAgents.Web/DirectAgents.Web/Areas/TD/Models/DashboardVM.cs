﻿using System;
using System.Collections.Generic;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Web.Areas.TD.Models
{
    public class DashboardVM
    {
        public DateTime Month { set { MonthString = value.ToShortDateString(); } }
        public string MonthString { get; set; }

        public IEnumerable<Campaign> Campaigns { get; set; }
    }
}