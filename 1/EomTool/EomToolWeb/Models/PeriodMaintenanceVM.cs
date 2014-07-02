using EomTool.Domain.Entities;
using System;
using System.Collections.Generic;

namespace EomToolWeb.Models
{
    public class PeriodMaintenanceVM
    {
        public String CurrentEomDateString { get; set; }

        public IEnumerable<Advertiser> NewAdvertisers { get; set; }
        public IEnumerable<Advertiser> ExpiredAdvertisers { get; set; }
        public IEnumerable<Advertiser> ChangedAdvertisers { get; set; }
    }
}