using System.Collections.Generic;
using System.Linq;
using System;

namespace EomApp1.Screens.Final.Models
{
    public class PublisherModelBase
    {
        private int pid;
        private string campaignName;
        private string advertiserName;

        public PublisherModelBase(int pid)
        {
            this.pid = pid;
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

        public int Pid
        {
            get { return pid; }
            set { pid = value; }
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

    public class PublishersModel : PublisherModelBase
    {
        public PublishersModel(int pid)
            : base(pid)
        {
        }

        public void FillPublishers(Data.DataSet1 data, CampaignStatusId campaignStatusID)
        {
            var campaign = data.Campaigns.Where(c => c.PID == this.Pid).FirstOrDefault();
            if (campaign == null)
            {
                campaign = data.Campaigns.AddCampaignsRow(this.Pid, this.CampaignName, 0, null);
            }

            using (var db = Models.EomEntities.Create())
            {
                var query = from affiliate in db.Affiliates
                            from item in db.Items
                            where affiliate.affid == item.affid && item.pid == this.Pid && item.campaign_status_id == (int)campaignStatusID
                            group item by new { Affiliate = affiliate, RevenueCurrency = item.Currency1 } into g
                            select new
                            {
                                AffId = g.Key.Affiliate.affid,
                                Affiliate = g.Key.Affiliate.name2,
                                Currency = g.Key.RevenueCurrency.name,
                                Total = g.Sum(c => c.total_revenue),
                                NetTerms = g.Key.Affiliate.NetTermType.name
                            };

                var publishers = new EomApp1.Screens.Final.Data.DataSet1.PublishersDataTable();

                foreach (var item in query)
                {
                    publishers.AddPublishersRow(item.Affiliate, item.Currency, item.Total.Value, item.AffId, item.NetTerms, campaign);

                }

                data.Publishers.Merge(publishers);
            }
        }

        public void ChangePublisherItems(CampaignStatusId fromStatusID, CampaignStatusId toStatusID, params int [] affIDs)
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
