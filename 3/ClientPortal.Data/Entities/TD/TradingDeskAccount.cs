using ClientPortal.Data.Contexts;
using ClientPortal.Data.Entities.TD.DBM;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ClientPortal.Data.Entities.TD
{
    public class TradingDeskAccount
    {
        public int TradingDeskAccountId { get; set; }

        public virtual ICollection<InsertionOrder> InsertionOrders { get; set; }

        [NotMapped]
        public IEnumerable<UserProfile> UserProfiles { get; set; }
        [NotMapped]
        public IEnumerable<Advertiser> Advertisers { get; set; }

        public int? InsertionOrderID()
        {
            int? ioID = null;
            if (InsertionOrders.Count > 0)
            {
                ioID = InsertionOrders.First().InsertionOrderID;
            }
            return ioID;
        }
    }
}
