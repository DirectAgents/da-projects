using System.Collections.Generic;
using DirectAgents.Domain.Entities;

namespace EomToolWeb.Models
{
    public class CampaignsListViewModel
    {
        public IEnumerable<Campaign> Campaigns { get; set; }
        public string SearchString { get; set; }
        public int? Pid { get; set; }
        public Country Country { get; set; }
        public Vertical Vertical { get; set; }
        public TrafficType TrafficType { get; set; }
        public string MobileLP { get; set; }

        public ListViewMode ListViewMode { get; set; }
    }
}