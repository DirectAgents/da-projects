using System.Collections.Generic;
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

    public class AdvertiserComparer : EqualityComparer<Advertiser>
    {
        public override bool Equals(Advertiser x, Advertiser y)
        {
            return x.AdvertiserId == y.AdvertiserId;
        }

        public override int GetHashCode(Advertiser obj)
        {
            return obj.AdvertiserId;
        }
    }
}
