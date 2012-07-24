using System.Collections.Generic;
using System.Linq;

namespace EomApp1.Screens.Final.Models
{
    public class PublishersModel
    {
        private int pid;
        private string campaignName;
        private string advertiserName;

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
                this.campaignName = campaign.campaign_name;
                this.advertiserName = campaign.Advertiser.name;
            }
        }

        public void FillPublishers(Data.PublishersDataSet.PublishersDataTable table, CampaignStatusId campaignStatusID)
        {
            using (var db = Models.EomEntities.Create())
            {
                var query = from affiliate in db.Affiliates
                            from item in db.Items
                            where affiliate.affid == item.affid && item.pid == this.pid && item.campaign_status_id == (int)campaignStatusID
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
                    table.AddPublishersRow(item.Affiliate, item.Currency, item.Total.Value, item.AffId, item.NetTerms, this.pid);
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

        public string CampaignName 
        {
            get
            {
                if (this.campaignName == null)
                {
                    Initialize();
                }
                return this.campaignName;
            }
        }

        public string AdvertiserName
        {
            get
            {
                if (this.advertiserName == null)
                {
                    Initialize();
                }
                return this.advertiserName;
            }
        }
    }
}
