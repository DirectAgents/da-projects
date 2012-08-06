using System.Linq;

namespace EomApp1.Screens.Final.Models
{
    using UI;
    using System;

    public class CampaignPublishers : CampaignPublishersBase
    {
        public CampaignPublishers(int pid, string currency)
            : base(pid, currency)
        {
        }

        public void Fill(Data data, CampaignStatusId campaignStatusID)
        {
            var campaign = data.Campaigns.Where(c => c.PID == Pid).FirstOrDefault();
            if (campaign == null)
            {
                campaign = data.Campaigns.AddCampaignsRow(Pid, CampaignName, 0, "", null, AdvertiserName, "");
            }

            using (var db = Eom.Create())
            {
                var query = from affiliate in db.Affiliates
                            from item in db.Items
                            where 
                                affiliate.affid == item.affid && 
                                item.pid == Pid && 
                                item.campaign_status_id == (int)campaignStatusID &&
                                item.Currency.name == this.Currency
                            group item by new { Affiliate = affiliate, RevenueCurrency = item.Currency, CostCurrency = item.Currency1 } into g
                            select new
                            {
                                AffId = g.Key.Affiliate.affid,
                                Affiliate = g.Key.Affiliate.name2,
                                Currency = g.Key.RevenueCurrency.name,
                                Total = g.Sum(i => i.total_revenue),
                                RevToUSD = g.Key.RevenueCurrency.to_usd_multiplier,
                                CostCurrency = g.Key.CostCurrency.name,
                                TotalCost = g.Sum(i => i.total_cost),
                                CostToUSD = g.Key.CostCurrency.to_usd_multiplier,
                                NetTerms = g.Key.Affiliate.NetTermType.name
                            };

                var merge = new Data.PublishersDataTable();
                foreach (var row in query)
                {
                    decimal marginPct = 0;
                    if (row.Total.Value != 0)
                    {
                        var revUSD = row.RevToUSD * row.Total.Value;
                        var costUSD = row.CostToUSD * row.TotalCost.Value;
                        marginPct = (1 - costUSD / revUSD);
                    }
                    merge.AddPublishersRow(row.Affiliate, row.Currency, row.Total.Value, row.AffId, row.NetTerms, campaign.PID, row.CostCurrency, row.TotalCost.Value, marginPct);

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
                    var query = from i in db.Items
                                where i.affid == affid && i.campaign_status_id == (int)fromStatusID && i.pid == Pid && i.Currency.name == Currency
                                select i;

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
