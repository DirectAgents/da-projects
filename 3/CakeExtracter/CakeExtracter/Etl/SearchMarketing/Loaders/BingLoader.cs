using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ClientPortal.Data.Contexts;

namespace CakeExtracter.Etl.SearchMarketing.Loaders
{
    public class BingLoader : Loader<Dictionary<string, string>>
    {
        private const string bingChannel = "Bing";
        private readonly int searchAccountId;

        public BingLoader(int searchAccountId)
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
                    var accountCode = item["AccountId"];
                    var campaignName = item["CampaignName"];
                    var campaignId = int.Parse(item["CampaignId"]);

                    var searchAccount = passedInAccount;
                    if (searchAccount.AccountCode != accountCode)
                        searchAccount = searchAccount.Advertiser.SearchAccounts.Single(sa => sa.AccountCode == accountCode && sa.Channel == bingChannel);

                    var pk1 = searchAccount.SearchCampaigns.Single(c => c.ExternalId == campaignId).SearchCampaignId;
                    var pk2 = DateTime.Parse(item["GregorianDate"]);
                    var pk3 = ".";
                    var pk4 = ".";
                    var pk5 = ".";
                    var source = new SearchDailySummary2
                    {
                        SearchCampaignId = pk1,
                        Date = pk2,
                        Network = pk3,
                        Device = pk4,
                        ClickType = pk5,
                        Revenue = decimal.Parse(item["Revenue"]),
                        Cost = decimal.Parse(item["Spend"]),
                        Orders = int.Parse(item["Conversions"]),
                        Clicks = int.Parse(item["Clicks"]),
                        Impressions = int.Parse(item["Impressions"]),
                        CurrencyId = 1 // item["CurrencyCode"] == "USD" ? 1 : -1 // NOTE: non USD (if exists) -1 for now
                    };
                    var target = db.Set<SearchDailySummary2>().Find(pk1, pk2, pk3, pk4, pk5);
                    if (target == null)
                    {
                        db.SearchDailySummary2.Add(source);
                        addedCount++;
                    }
                    else
                    {
                        AutoMapper.Mapper.Map(source, target);
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

                var accountTuples = items.Select(i => Tuple.Create(i["AccountName"], i["AccountId"])).Distinct();
                bool multipleAccounts = accountTuples.Count() > 1;

                foreach (var tuple in accountTuples)
                {
                    var accountName = "Bing " + tuple.Item1;
                    var accountCode = tuple.Item2;
                    SearchAccount existing = null;

                    if (searchAccount.AccountCode == accountCode || !multipleAccounts)
                    {   // if the accountID matches or if there's only one account in the items-to-load, use the passed-in SearchAccount
                        existing = searchAccount;
                    }
                    else
                    {   // See if there are any sibling SearchAccounts that match by accountCode... or finally, by name
                        existing = searchAccount.Advertiser.SearchAccounts.SingleOrDefault(sa => sa.AccountCode == accountCode && sa.Channel == bingChannel);
                        if (existing == null)
                            existing = searchAccount.Advertiser.SearchAccounts.SingleOrDefault(sa => sa.Name == accountName && sa.Channel == bingChannel);
                    }

                    if (existing == null)
                    {
                        searchAccount.Advertiser.SearchAccounts.Add(new SearchAccount
                        {
                            Name = accountName,
                            Channel = bingChannel,
                            AccountCode = accountCode
                        });
                        Logger.Info("Saving new SearchAccount: {0} ({1})", accountName, accountCode);
                    }
                    else
                    {
                        if (existing.Name != accountName)
                        {
                            existing.Name = accountName;
                            Logger.Info("Saving updated SearchAccount name: {0} ({1})", accountName, existing.SearchAccountId);
                        }
                        if (existing.AccountCode != accountCode)
                        {
                            existing.AccountCode = accountCode;
                            Logger.Info("Saving updated SearchAccount AccountCode: {0} ({1})", accountCode, existing.SearchAccountId);
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

                foreach (var tuple in items.Select(c => Tuple.Create(c["AccountId"], c["CampaignName"], c["CampaignId"])).Distinct())
                {
                    var accountCode = tuple.Item1;
                    var campaignName = tuple.Item2;
                    var campaignId = int.Parse(tuple.Item3);

                    var searchAccount = passedInAccount;
                    if (searchAccount.AccountCode != accountCode)
                        searchAccount = searchAccount.Advertiser.SearchAccounts.Single(sa => sa.AccountCode == accountCode && sa.Channel == bingChannel);

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
                    else if (existing.SearchCampaignName != campaignName)
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
