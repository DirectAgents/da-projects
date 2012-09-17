using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirectAgents.Domain.Entities;

namespace EomToolWeb.Models
{
    public class CampaignsListViewModel
    {
        public IEnumerable<Campaign> Campaigns { get; set; }
        public Country Country { get; set; }
        public Vertical Vertical { get; set; }
    }
}