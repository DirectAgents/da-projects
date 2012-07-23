using System.Collections.Generic;
using System.Linq;

namespace EomApp1.Screens.Final.Models
{
    public class PublishersModel
    {
        private int pid;

        public PublishersModel(int pid)
        {
            this.pid = pid;
            Initialize();
        }

        private void Initialize()
        {
            using (var db = Models.EomEntities.Create())
            {
                var campaign = db.Campaigns.Where(c => c.pid == this.pid).Single();
                this.CampaignName = campaign.campaign_name;
                this.AdvertiserName = campaign.Advertiser.name;
            }
        }

        public string CampaignName { get; set; }

        public string AdvertiserName { get; set; }

        public void FillPublishers(Data.PublishersDataSet.PublishersDataTable table, CampaignStatusId campaignStatusID)
        {
            using (var db = Models.EomEntities.Create())
            {
                var query = from affiliate in db.Affiliates
                            from item in db.Items
                            where affiliate.affid == item.affid && item.pid == pid && item.campaign_status_id == (int)campaignStatusID
                            group item by new { Affiliate = affiliate, RevenueCurrency = item.Currency1 } into g
                            select new
                            {
                                AffId = g.Key.Affiliate.affid,
                                Affiliate = g.Key.Affiliate.name,
                                Currency = g.Key.RevenueCurrency.name,
                                Total = g.Sum(c => c.total_revenue),
                                NetTerms = g.Key.Affiliate.NetTermType.name
                            };

                foreach (var item in query)
                {
                    table.AddPublishersRow(item.Affiliate, item.Currency, item.Total.Value, item.AffId, item.NetTerms);
                }
            }
        }

        public void ChangePublisherItems(List<int> affIDs, CampaignStatusId fromStatusID, CampaignStatusId toStatusID)
        {
            using (var db = Models.EomEntities.Create())
            {
                foreach (var affid in affIDs)
                {
                    var query = from c in db.Items
                                where c.affid == affid && c.campaign_status_id == (int)fromStatusID
                                select c;

                    foreach (var item in query)
                    {
                        item.campaign_status_id = (int)toStatusID;
                    }
                }

                db.SaveChanges();
            }
        }
    }
}
