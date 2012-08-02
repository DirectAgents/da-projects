using System.Linq;

namespace EomApp1.Screens.Final.Models
{
    using UI;

    public class CampaignPublishers : CampaignPublishersBase
    {
        public CampaignPublishers(int pid)
            : base(pid)
        {
        }

        public void Fill(Data data, CampaignStatusId campaignStatusID)
        {
            var campaign = data.Campaigns.Where(c => c.PID == Pid).FirstOrDefault();
            if (campaign == null)
            {
                campaign = data.Campaigns.AddCampaignsRow(Pid, CampaignName, 0, null, null, AdvertiserName);
            }

            using (var db = Eom.Create())
            {
                var query = from affiliate in db.Affiliates
                            from item in db.Items
                            where affiliate.affid == item.affid && item.pid == Pid && item.campaign_status_id == (int)campaignStatusID
                            group item by new { Affiliate = affiliate, RevenueCurrency = item.Currency } into g
                            select new
                            {
                                AffId = g.Key.Affiliate.affid,
                                Affiliate = g.Key.Affiliate.name2,
                                Currency = g.Key.RevenueCurrency.name,
                                Total = g.Sum(c => c.total_revenue),
                                NetTerms = g.Key.Affiliate.NetTermType.name
                            };

                var merge = new Data.PublishersDataTable();
                foreach (var row in query)
                {
                    merge.AddPublishersRow(row.Affiliate, row.Currency, row.Total.Value, row.AffId, row.NetTerms, campaign.PID);

                }
                data.Publishers.Merge(merge);
            }
        }

        public void ChangeCampaignStatus(CampaignStatusId fromStatusID, CampaignStatusId toStatusID, params int[] ids)
        {
            using (var db = Models.Eom.Create())
            {
                foreach (var affid in ids)
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
