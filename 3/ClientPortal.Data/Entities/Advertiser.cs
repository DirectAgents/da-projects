﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ClientPortal.Data.Contexts
{
    public partial class Advertiser
    {
        [NotMapped]
        public IOrderedEnumerable<AdvertiserContact> AdvertiserContactsOrdered
        {
            get { return this.AdvertiserContacts.OrderBy(ac => ac.Order); }
        }

        [NotMapped]
        public IEnumerable<UserProfile> UserProfiles { get; set; }
    }
}
