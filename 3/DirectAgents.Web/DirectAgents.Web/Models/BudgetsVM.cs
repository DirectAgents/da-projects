using DirectAgents.Domain.Entities.Cake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirectAgents.Web.Models
{
    public class BudgetsVM
    {
        public IEnumerable<Advertiser> Advertisers { get; set; }
        public Contact AccountManager { get; set; }
        public bool ShowAll { get; set; }
    }
}