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

        public AdWordsApiLoader(int searchAccountId)
        {
            this.searchAccountId = searchAccountId;
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
            using (var db = new ClientPortalContext())
            {
                var passedInAccount = db.SearchAccounts.Find(this.searchAccountId);

                foreach (var item in items)
                {
                    var accountId = item["accountID"];
                    var campaignId = int.Parse(item["campaignID"]);

                    var searchAccount = passedInAccount;
                    if (searchAccount.ExternalId != accountId)
                        searchAccount = searchAccount.Advertiser.SearchAccounts.Single(sa => sa.ExternalId == accountId && sa.Channel == googleChannel);

                    var pk1 = searchAccount.SearchCampaigns.Single(c => c.ExternalId == campaignId).SearchCampaignId;
                    var pk2 = DateTime.Parse(item["day"].Replace('-', '/'));
                    var pk3 = item["network"].Substring(0, 1);
                    var pk4 = item["device"].Substring(0, 1);
                    var pk5 = item["clickType"].Substring(0, 1);
                    var source = new SearchDailySummary2
                    {
                        SearchCampaignId = pk1,
                        Date = pk2,
                        Network = pk3,
                        Device = pk4,
                        ClickType = pk5,
                        Revenue = decimal.Parse(item["totalConvValue"]),
                        Cost = decimal.Parse(item["cost"]) / 1000000, // convert from mincrons to dollars
                        Orders = int.Parse(item["conv1PerClick"]),
                        Clicks = int.Parse(item["clicks"]),
                        Impressions = int.Parse(item["impressions"]),
                        CurrencyId = (!item.Keys.Contains("currency") || item["currency"] == "USD") ? 1 : -1 // NOTE: non USD (if exists) -1 for now
                    };

                    // HACK: ignoring impressions for rows that do not have H as click type
                    if (source.ClickType != "H")
                    {
                        source.Impressions = 0;
                    }

                    var target = db.Set<SearchDailySummary2>().Find(pk1, pk2, pk3, pk4, pk5);
                    if (target == null)
                    {
                        db.SearchDailySummary2.Add(source);
                        addedCount++;
                    }
                    else
                    {
                        db.Entry(target).State = EntityState.Detached;
                        target = AutoMapper.Mapper.Map(source, target);
                        db.Entry(target).State = EntityState.Modified;
                        updatedCount++;
                    }
                    itemCount++;
                }
                Logger.Info("Saving {0} SearchDailySummaries ({1} updates, {2} additions)", itemCount, updatedCount, addedCount);
                db.SaveChanges();
            }
            return itemCount;
        }

        private void AddUpdateDependentSearchAccounts(List<Dictionary<string, string>> items)
        {
            using (var db = new ClientPortalContext())
            {
                var searchAccount = db.SearchAccounts.Find(this.searchAccountId);

                var accountTuples = items.Select(i => Tuple.Create(i["account"], i["accountID"])).Distinct();
                bool multipleAccounts = accountTuples.Count() > 1;

                foreach (var tuple in accountTuples)
                {
                    var accountName = tuple.Item1;
                    var accountID = tuple.Item2;
                    SearchAccount existing = null;

                    if (searchAccount.ExternalId == accountID || !multipleAccounts)
                    {   // if the accountID matches or if there's only one account in the items-to-load, use the passed-in SearchAccount
                        existing = searchAccount;
                    }
                    else
                    {   // See if there are any sibling SearchAccounts that match by accountID... or finally, by name
                        existing = searchAccount.Advertiser.SearchAccounts.SingleOrDefault(sa => sa.ExternalId == accountID && sa.Channel == googleChannel);
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
                            ExternalId = accountID
                        });
                        Logger.Info("Saving new SearchAccount: {0} ({1})", accountName, accountID);
                    }
                    else
                    {
                        if (existing.Name != accountName)
                        {
                            existing.Name = accountName;
                            Logger.Info("Saving updated SearchAccount name: {0} ({1})", accountName, existing.SearchAccountId);
                        }
                        if (existing.ExternalId != accountID)
                        {
                            existing.ExternalId = accountID;
                            Logger.Info("Saving updated SearchAccount ExternalId: {0} ({1})", accountID, existing.SearchAccountId);
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

                foreach (var tuple in items.Select(c => Tuple.Create(c["accountID"], c["campaign"], c["campaignID"])).Distinct())
                {
                    var accountId = tuple.Item1;
                    var campaignName = tuple.Item2;
                    var campaignId = int.Parse(tuple.Item3);

                    var searchAccount = passedInAccount;
                    if (searchAccount.ExternalId != accountId)
                        searchAccount = searchAccount.Advertiser.SearchAccounts.Single(sa => sa.ExternalId == accountId && sa.Channel == googleChannel);

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
