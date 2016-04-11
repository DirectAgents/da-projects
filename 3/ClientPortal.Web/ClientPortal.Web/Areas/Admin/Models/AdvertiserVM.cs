using System;
using System.Collections.Generic;
using System.Linq;
using ClientPortal.Data.Contexts;

namespace ClientPortal.Web.Areas.Admin.Models
{
    public class AdvertiserVM
    {
        public DirectAgents.Domain.Entities.TD.Advertiser ProgAdvertiser { get; set; }
        public IEnumerable<UserProfile> UserProfiles { get; set; }

        public string Login
        {
            get
            {
                if (UserProfiles == null)
                    return "";
                return String.Join(", ", UserProfiles.Select(u => u.UserName).ToArray());
                // (normally there should be just one)
            }
        }
    }
}