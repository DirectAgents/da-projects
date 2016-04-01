using System;
using System.Collections.Generic;
using System.Linq;
using ClientPortal.Data.Contexts;

namespace ClientPortal.Web.Areas.Admin.Models
{
    public class UserVM
    {
        public UserProfile UserProfile { get; set; }
        public Advertiser CakeAdvertiser { get; set; }
        public DirectAgents.Domain.Entities.TD.Advertiser ProgAdvertiser { get; set; }
    }
}