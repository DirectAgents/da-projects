using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ClientPortal.Data.Contexts;

namespace CakeExtracter.Etl.SearchMarketing.Loaders
{
    public class AdWordsApiLoader : Loader<Dictionary<string, string>>
    {
        private const string googleChannel = "Google";
        private readonly int searchAccountId;
        private readonly bool includeBreakdown;

        public AdWordsApiLoader(int searchAccountId, bool includeBreakdown = false)
        {
            this.searchAccountId = searchAccountId;
            this.includeBreakdown = includeBreakdown;
        }

        protected override int Load(List<Dictionary<string, string>> items)
        {
            Logger.Info("Loading {0} SearchDailySummaries..", items.Count);
            AddUpdateDependentSearchAccounts(items);
            AddUpdateDependentSearchCampaigns(items);
            var count = UpsertSearchDailySummaries(items);
            return count;
        }

        private int UpsertSearchDailySummaries(List<Dictionary<string, string>> items)
        {
            var addedCount = 0;
            var updatedCount = 0;
            var itemCount = 0;
            //var networks = items.Select(i => i["network"]).Distinct();
            //var devices = items.Select(i => i["device"]).Distinct();
            //var clickTypes = items.Select(i => i["clickType"]).Distinct();
            //var pla = items.Where(i => i["clickType"] == "Product listing ad").OrderBy(i => i["campaignID"]);
            //var plac = items.Where(i => i["clickType"] == "Product Listing Ad - Coupon").OrderBy(i => i["campaignID"]);
            using (var db = new ClientPortalContext())
            {
                var passedInAccount = db.SearchAccounts.Find(this.searchAccountId);

                foreach (var item in items)
                {
                    var customerId = item["customerID"];
                    var campaignId = int.Parse(item["campaignID"]);

                    var searchAccount = passedInAccount;
                    if (searchAccount.ExternalId != customerId)
                        searchAccount = searchAccount.Advertiser.SearchAccounts.Single(sa => sa.ExternalId == customerId && sa.Channel == googleChannel);

                    var sds = new SearchDailySummary
                    {
                        SearchCampaignId = searchAccount.SearchCampaigns.Single(c => c.ExternalId == campaignId).SearchCampaignId,
                        Date = DateTime.Parse(item["day"].Replace('-', '/')),
                        Revenue = decimal.Parse(item["totalConvValue"]),
                        Cost = decimal.Parse(item["cost"]) / 1000000, // convert from mincrons to dollars
                        Orders = int.Parse(item["convertedClicks"]),
                        Clicks = int.Parse(item["clicks"]),
                        Impressions = int.Parse(item["impressions"]),
                        CurrencyId = (!item.Keys.Contains("currency") || item["currency"] == "USD") ? 1 : -1 // NOTE: non USD (if exists) -1 for now
                    };

                    bool added;
                    if (includeBreakdown)
                    {
                        var clickTypeAbbrev = item["clickType"].Substring(0, 1);
                        if (item["clickType"].ToLower() == "product listing ad - coupon") // started on 10/18/14 for Folica|Search (conflict with "Product listing ad")
                            clickTypeAbbrev = "Q";
                        added = UpsertSearchDailySummary2(db, sds, item["network"].Substring(0, 1), item["device"].Substring(0, 1), clickTypeAbbrev);
                    }
                    else
                        added = UpsertSearchDailySummary(db, sds);

                    if (added)
                        addedCount++;
                    else
                        updatedCount++;

                    itemCount++;
                }
                Logger.Info("Saving {0} SearchDailySummaries ({1} updates, {2} additions)", itemCount, updatedCount, addedCount);
                db.SaveChanges();
            }
            return itemCount;
        }

        // return true if added; false if updated
        private bool UpsertSearchDailySummary(ClientPortalContext db, SearchDailySummary sds)
        {
            var target = db.Set<SearchDailySummary>().Find(sds.SearchCampaignId, sds.Date);
            if (target == null)
            {
                db.SearchDailySummaries.Add(sds);
                return true;
            }
            else
            {
                db.Entry(target).State = EntityState.Detached;
                target = AutoMapper.Mapper.Map(sds, target);
                db.Entry(target).State = EntityState.Modified;
                return false;
            }
        }
        private bool UpsertSearchDailySummary2(ClientPortalContext db, SearchDailySummary sds, string network, string device, string clickType)
        {
            var sds2 = new SearchDailySummary2
            {
                SearchCampaignId = sds.SearchCampaignId,
                Date = sds.Date,
                Network = network,
                Device = device,
                ClickType = clickType,
                Revenue = sds.Revenue,
                Cost = sds.Cost,
                Orders = sds.Orders,
                Clicks = sds.Clicks,
                // HACK: ignoring impressions for rows that do not have H as click type
                Impressions = (clickType == "H") ? sds.Impressions : 0,
                CurrencyId = sds.CurrencyId
            };

            var target = db.Set<SearchDailySummary2>().Find(sds2.SearchCampaignId, sds2.Date, sds2.Network, sds2.Device, sds2.ClickType);
            if (target == null)
            {
                db.SearchDailySummary2.Add(sds2);
                return true;
            }
            else
            {
                db.Entry(target).State = EntityState.Detached;
                target = AutoMapper.Mapper.Map(sds2, target);
                db.Entry(target).State = EntityState.Modified;
                return false;
            }
        }

        private void AddUpdateDependentSearchAccounts(List<Dictionary<string, string>> items)
        {
            using (var db = new ClientPortalContext())
            {
                var searchAccount = db.SearchAccounts.Find(this.searchAccountId);

                var accountTuples = items.Select(i => Tuple.Create(i["account"], i["customerID"])).Distinct();
                bool multipleAccounts = accountTuples.Count() > 1;

                foreach (var tuple in accountTuples)
                {
                    var accountName = tuple.Item1;
                    var customerId = tuple.Item2;
                    SearchAccount existing = null;

                    if (searchAccount.ExternalId == customerId || !multipleAccounts)
                    {   // if the customerId matches or if there's only one account in the items-to-load, use the passed-in SearchAccount
                        existing = searchAccount;
                    }
                    else
                    {   // See if there are any sibling SearchAccounts that match by customerId... or finally, by name
                        existing = searchAccount.Advertiser.SearchAccounts.SingleOrDefault(sa => sa.ExternalId == customerId && sa.Channel == googleChannel);
                        if (existing == null)
                            existing = searchAccount.Advertiser.SearchAccounts.SingleOrDefault(sa => sa.Name == accountName && sa.Channel == googleChannel);
                    }

                    if (existing == null)
                    {
                        searchAccount.Advertiser.SearchAccounts.Add(new SearchAccount
                        {
                            Name = accountName,
                            Channel = googleChannel,
                            //AccountCode = , // todo: have extracter get client code
                            ExternalId = customerId
                        });
                        Logger.Info("Saving new SearchAccount: {0} ({1})", accountName, customerId);
                    }
                    else
                    {
                        if (existing.Name != accountName)
                        {
                            existing.Name = accountName;
                            Logger.Info("Saving updated SearchAccount name: {0} ({1})", accountName, existing.SearchAccountId);
                        }
                        if (existing.ExternalId != customerId)
                        {
                            existing.ExternalId = customerId;
                            Logger.Info("Saving updated SearchAccount ExternalId: {0} ({1})", customerId, existing.SearchAccountId);
                        }
                    }
                    db.SaveChanges();
                }
            }
        }

        private void AddUpdateDependentSearchCampaigns(List<Dictionary<string, string>> items)
        {
            using (var db = new ClientPortalContext())
            {
                var passedInAccount = db.SearchAccounts.Find(this.searchAccountId);

                foreach (var tuple in items.Select(c => Tuple.Create(c["customerID"], c["campaign"], c["campaignID"])).Distinct())
                {
                    var customerId = tuple.Item1;
                    var campaignName = tuple.Item2;
                    var campaignId = int.Parse(tuple.Item3);

                    var searchAccount = passedInAccount;
                    if (searchAccount.ExternalId != customerId)
                        searchAccount = searchAccount.Advertiser.SearchAccounts.Single(sa => sa.ExternalId == customerId && sa.Channel == googleChannel);

                    var existing = searchAccount.SearchCampaigns.SingleOrDefault(c => c.ExternalId == campaignId);

                    if (existing == null)
                    {
                        searchAccount.SearchCampaigns.Add(new SearchCampaign
                        {
                            SearchCampaignName = campaignName,
                            ExternalId = campaignId
                        });
                        Logger.Info("Saving new SearchCampaign: {0} ({1})", campaignName, campaignId);
                        db.SaveChanges();
                    }
                    else if(existing.SearchCampaignName != campaignName)
                    {
                        existing.SearchCampaignName = campaignName;
                        Logger.Info("Saving updated SearchCampaign name: {0} ({1})", campaignName, campaignId);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
