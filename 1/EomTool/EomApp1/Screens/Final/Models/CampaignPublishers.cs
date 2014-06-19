using System.Linq;

namespace EomApp1.Screens.Final.Models
{
    using UI;
    using System;
    using EomAppCommon;
    using System.Collections.Generic;

    public class CampaignPublishers : CampaignPublishersBase
    {
        public CampaignPublishers(int pid, string currency)
            : base(pid, currency)
        {
        }

        public void Fill(Data data, CampaignStatusId campaignStatusID, MediaBuyerApprovalStatusId? mediaBuyerStatusID = null)
        {
            var campaign = data.Campaigns.Where(c => c.PID == Pid).FirstOrDefault();
            if (campaign == null)
            {
                campaign = data.Campaigns.AddCampaignsRow(Pid, CampaignName, 0, "", null, AdvertiserName);
            }

            using (var db = Eom.Create())
            {
                var items = GetItems(db, campaignStatusID, null, null, mediaBuyerStatusID);

                var query = from affiliate in db.Affiliates
                            from item in items
                            where 
                                affiliate.affid == item.affid
                            group item by new { Affiliate = affiliate, RevenueCurrency = item.RevenueCurrency, CostCurrency = item.CostCurrency } into g
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

        public void CheckFinalizationMargins(int[] affids, string[] costcurrs, out int[] rejectedAffIds)
        {
            var minimumMarginPct = (EomAppSettings.Settings.EomAppSettings_FinalizationWorkflow_MinimumMargin ?? 0) / 100;
            List<int> rejectedAffIdList = new List<int>();

            using (var db = Models.Eom.Create())
            {
                for (var j = 0; j < affids.Length; j++)
                {
                    string costcurr = null;
                    if (j < costcurrs.Length)
                        costcurr = costcurrs[j];

                    var items = GetItems(db, CampaignStatusId.Default, affids[j], costcurr);
                    var rows = from affiliate in db.Affiliates
                               from item in items
                               where
                                   affiliate.affid == item.affid
                               group item by new { affiliate, item.RevenueCurrency, item.CostCurrency } into g
                               select new
                               {
                                   AffId = g.Key.affiliate.affid,
                                   MarginExempt = g.Key.affiliate.margin_exempt,
                                   TotalRevenue = g.Sum(i => i.total_revenue ?? 0),
                                   RevToUSD = g.Key.RevenueCurrency.to_usd_multiplier,
                                   TotalCost = g.Sum(i => i.total_cost ?? 0),
                                   CostToUSD = g.Key.CostCurrency.to_usd_multiplier
                               };

                    // Generally there will just be one row - because we're doing one affiliate at a time
                    foreach (var row in rows)
                    {
                        if (row.MarginExempt)
                            continue;

                        decimal marginPct = 0;
                        if (row.TotalRevenue != 0)
                        {
                            var revUSD = row.RevToUSD * row.TotalRevenue;
                            var costUSD = row.CostToUSD * row.TotalCost;
                            marginPct = (1 - costUSD / revUSD);
                        }
                        if (marginPct < minimumMarginPct || marginPct <= 0 || marginPct >= 1)
                            rejectedAffIdList.Add(row.AffId);
                    }
                }
            }
            rejectedAffIds = rejectedAffIdList.ToArray();
        }

        public void ChangeCampaignStatus(CampaignStatusId fromStatusID, CampaignStatusId toStatusID, int[] affids, string[] costcurrs, MediaBuyerApprovalStatusId? mbApprovalStatusId = null)
        {
            using (var db = Models.Eom.Create())
            {
                for (var j = 0; j < affids.Length; j++)
                {
                    string costcurr = null;
                    if (j < costcurrs.Length)
                        costcurr = costcurrs[j];

                    var items = GetItems(db, fromStatusID, affids[j], costcurr, mbApprovalStatusId);

                    foreach (var item in items)
                    {
                        item.campaign_status_id = (int)toStatusID;
                    }
                }

                db.SaveChanges();
            }
        }

        private IQueryable<Item> GetItems(Eom db, CampaignStatusId campaignStatusId, int? affid = null, string costcurr = null, MediaBuyerApprovalStatusId? mbApprovalStatusId = null)
        {
            var items = from i in db.Items
                        where i.pid == Pid && i.RevenueCurrency.name == Currency
                           && i.campaign_status_id == (int)campaignStatusId
                        select i;
            if (affid.HasValue)
            {
                items = items.Where(i => i.affid == affid.Value);
            }
            if (costcurr != null)
            {
                items = items.Where(i => i.CostCurrency.name == costcurr);
            }
            if (mbApprovalStatusId != null)
            {
                items = items.Where(i => i.media_buyer_approval_status_id == (int)mbApprovalStatusId);
            }
            return items;
        }
    }
}
