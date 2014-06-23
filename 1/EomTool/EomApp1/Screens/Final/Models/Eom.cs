using EomAppCommon;
using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;

namespace EomApp1.Screens.Final.Models
{
    public partial class Eom
    {
        public static Eom Create()
        {
            return new Eom(new EntityConnectionStringBuilder {
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = EomAppSettings.ConnStr + ";multipleactiveresultsets=True;App=EntityFramework",
                Metadata = @"res://*/Screens.Final.Models.EomModel.csdl|res://*/Screens.Final.Models.EomModel.ssdl|res://*/Screens.Final.Models.EomModel.msl"
            }.ConnectionString);
        }

        public static IQueryable<Item> GetItems(Eom db, int pid, string revcurr, CampaignStatusId campaignStatusId, int? affid = null, string costcurr = null, MediaBuyerApprovalStatusId? mbApprovalStatusId = null)
        {
            var items = from i in db.Items
                        where i.pid == pid && i.RevenueCurrency.name == revcurr
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

        public static void CheckFinalizationMargins(int pid, string revcurr, int[] affids, string[] costcurrs, out int[] rejectedAffIds)
        {
            var minimumMarginPct = (EomAppSettings.Settings.EomAppSettings_FinalizationWorkflow_MinimumMargin ?? 0) / 100;
            int[] affIdsRequireNegativeApproval = EomAppSettings.Settings.EomAppSettings_FinalizationWorkflow_AffIdsAlwaysRequireApprovalForNegativeMargin;
            List<int> rejectedAffIdList = new List<int>();
            var now = DateTime.Now;

            using (var db = Models.Eom.Create())
            {
                var items = Eom.GetItems(db, pid, revcurr, CampaignStatusId.Default); // get unfinalized items for this campaign & revcurr

                if (affids == null) // means: check all affiliates
                {
                    affids = items.Select(i => i.affid).Distinct().ToArray();
                }
                for (var j = 0; j < affids.Length; j++)
                {
                    var affid = affids[j];
                    var itemsNarrowed = items.Where(i => i.affid == affid);

                    if (costcurrs != null && j < costcurrs.Length)
                    {
                        var costcurr = costcurrs[j];
                        itemsNarrowed = itemsNarrowed.Where(i => i.CostCurrency.name == costcurr);
                    }
                    //var items = Eom.GetItems(db, pid, revcurr, CampaignStatusId.Default, affids[j], costcurr);
                    var rows = from affiliate in db.Affiliates
                               from item in itemsNarrowed
                               where
                                   affiliate.affid == item.affid
                               group item by new { affiliate, item.RevenueCurrency, item.CostCurrency } into g
                               select new
                               {
                                   AffId = g.Key.affiliate.affid,
                                   MarginExempt = g.Key.affiliate.margin_exempt,
                                   RevenueCurrency = g.Key.RevenueCurrency,
                                   TotalRevenue = g.Sum(i => i.total_revenue ?? 0),
                                   RevToUSD = g.Key.RevenueCurrency.to_usd_multiplier,
                                   CostCurrency = g.Key.CostCurrency,
                                   TotalCost = g.Sum(i => i.total_cost ?? 0),
                                   CostToUSD = g.Key.CostCurrency.to_usd_multiplier
                               };

                    // Generally there will just be one row - because we're doing one affiliate at a time; but it's possible to have multiple cost currs
                    foreach (var row in rows)
                    {
                        // Skip affiliates that are "exempt" - except for special cases
                        if (row.MarginExempt && !affIdsRequireNegativeApproval.Contains(row.AffId))
                            continue;

                        bool approvalNeeded = false;
                        if (row.TotalRevenue != 0)
                        {
                            var revUSD = row.RevToUSD * row.TotalRevenue;
                            var costUSD = row.CostToUSD * row.TotalCost;
                            var marginPct = (1 - costUSD / revUSD);
                            approvalNeeded = (marginPct < minimumMarginPct || marginPct <= 0 || marginPct >= 1);

                            if (row.MarginExempt && affIdsRequireNegativeApproval.Contains(row.AffId) && marginPct >= 0)
                                approvalNeeded = false; // special cases - only exempt from negative margin approval
                        }
                        else if (row.TotalCost != 0 && (!row.MarginExempt || affIdsRequireNegativeApproval.Contains(row.AffId)))
                        {
                            approvalNeeded = true; // revenue is 0; cost is not 0; requires approval
                        }

                        if (approvalNeeded)
                        {  // See if there's an unused marginApproval. if so, set it to used (and don't add to rejectedAffIdList)
                            var marginApproval = db.MarginApprovals.Where(ma => ma.pid == pid && ma.affid == row.AffId && !ma.used.HasValue &&
                                                                          ma.RevenueCurrency.name == row.RevenueCurrency.name && ma.total_revenue == row.TotalRevenue &&
                                                                          ma.CostCurrency.name == row.CostCurrency.name && ma.total_cost == row.TotalCost).FirstOrDefault();
                            if (marginApproval != null)
                                marginApproval.used = now;
                            else
                                rejectedAffIdList.Add(row.AffId);
                        }
                    }
                } // loop through affids

                if (rejectedAffIdList.Count == 0)
                    db.SaveChanges(); // save any MarginApprovals that were marked as used
            }
            rejectedAffIds = rejectedAffIdList.ToArray();
        }

    }
}
