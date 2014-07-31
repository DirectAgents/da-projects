using ClientPortal.Data.Contexts;
using ClientPortal.Data.Entities.TD.DBM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ClientPortal.Data.Entities.TD
{
    public class TradingDeskAccount
    {
        public int TradingDeskAccountId { get; set; }
        public bool ShowConversions { get; set; }
        public string FixedMetricName { get; set; }
        public decimal? FixedMetricValue { get; set; }

        public virtual ICollection<InsertionOrder> InsertionOrders { get; set; }

        [NotMapped]
        public IEnumerable<UserProfile> UserProfiles { get; set; }
        [NotMapped]
        public IEnumerable<Advertiser> Advertisers { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                if (Advertisers != null && Advertisers.Count() > 0)
                    return String.Join(", ", Advertisers.Select(a => a.AdvertiserName).ToArray());
                else if (UserProfiles != null && UserProfiles.Count() > 0)
                    return String.Join(", ", UserProfiles.Select(u => u.UserName).ToArray());
                else
                    return TradingDeskAccountId.ToString();
            }
        }
        [NotMapped]
        public string FixedMetricDisplay
        {
            get
            {
                if (String.IsNullOrWhiteSpace(FixedMetricName))
                    return "(none)";
                //else if (FixedMetricName == "CPM" || FixedMetricName == "CPC")
                //    return String.Format("{0}: {1:C}", FixedMetricName, FixedMetricValue);
                else
                    return String.Format("{0}: {1:0.##########}", FixedMetricName, FixedMetricValue);
            }
        }

        //Usually there is just one InsertionOrder per TDAccount
        public int? InsertionOrderID()
        {
            int? ioID = null;
            if (InsertionOrders.Count > 0)
            {
                ioID = InsertionOrders.First().InsertionOrderID;
            }
            return ioID;
        }

        public IEnumerable<int> AdvertiserIds()
        {
            if (UserProfiles == null)
                return new List<int>();
            var advIds = UserProfiles.Where(up => up.CakeAdvertiserId.HasValue).Select(up => up.CakeAdvertiserId.Value);
            return advIds;
        }
    }
}
