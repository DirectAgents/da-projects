﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPSearch;

namespace CakeExtracter.Etl.SearchMarketing.Loaders
{
    public class BingLoader : Loader<Dictionary<string, string>>
    {
        public const string BingChannel = "Bing";
        private readonly int searchAccountId;
        private readonly decimal? revenuePerOrder;
        private bool hasAccountId = false;

        public BingLoader(int searchAccountId, decimal? revenuePerOrder = null)
        {
            this.searchAccountId = searchAccountId;
            this.revenuePerOrder = revenuePerOrder;
        }
        //TODO: if !hasAccountId, match below on "account number" -> external id ?
        //NOTE: not handled is the case when some items have accountIds and some don't

        protected override int Load(List<Dictionary<string, string>> items)
        {
            Logger.Info("Loading {0} SearchDailySummaries..", items.Count);
            if (items.Count == 0) return 0;

            hasAccountId = items[0].ContainsKey("AccountId");
            if (hasAccountId)
                BingLoader.AddUpdateDependentSearchAccounts(items, this.searchAccountId);
            BingLoader.AddUpdateDependentSearchCampaigns(items, this.searchAccountId);
            var count = UpsertSearchDailySummaries(items);
            return count;
        }

        private int UpsertSearchDailySummaries(List<Dictionary<string, string>> items)
        {
            const string network = ".";
            const string device = ".";
            var progress = new LoadingProgress();

            using (var db = new ClientPortalSearchContext())
            {
                var passedInAccount = db.SearchAccounts.Find(this.searchAccountId);

                foreach (var item in items)
                {
                    //var campaignName = item["CampaignName"];
                    var campaignId = item["CampaignId"];

                    var searchAccount = passedInAccount;
                    if (hasAccountId)
                    {
                        var accountCode = item["AccountId"];
                        if (searchAccount.AccountCode != accountCode)
                            searchAccount = searchAccount.SearchProfile.SearchAccounts.Single(sa => sa.AccountCode == accountCode && sa.Channel == BingChannel);
                    }

                    var searchCampaignId = searchAccount.SearchCampaigns.Single(c => c.ExternalId == campaignId).SearchCampaignId;
                    var time = DateTime.Parse(item["TimePeriod"]);
                    var source = new SearchDailySummary
                    {
                        SearchCampaignId = searchCampaignId,
                        Date = time,
                        Network = network,
                        Device = device,
                        Revenue = decimal.Parse(item["Revenue"]),
                        Cost = decimal.Parse(item["Spend"]),
                        Orders = int.Parse(item["Conversions"]),
                        Clicks = int.Parse(item["Clicks"]),
                        Impressions = int.Parse(item["Impressions"]),
                        CurrencyId = 1 // item["CurrencyCode"] == "USD" ? 1 : -1 // NOTE: non USD (if exists) -1 for now
                    };

                    // RevPerOrder: the value set in the constructor takes precedence, followed by a value in the SearchAccount
                    if (revenuePerOrder.HasValue)
                        source.Revenue = source.Orders * revenuePerOrder.Value;
                    else if (searchAccount.RevPerOrder.HasValue)
                        source.Revenue = source.Orders * searchAccount.RevPerOrder.Value;

                    var target = db.Set<SearchDailySummary>().Find(searchCampaignId, time, network, device);
                    if (target == null)
                    {
                        db.SearchDailySummaries.Add(source);
                        progress.AddedCount++;
                    }
                    else
                    {
                        var entry = db.Entry(target);
                        if (entry.State != EntityState.Unchanged)
                        {
                            Logger.Warn(accountId, "Encountered duplicate for {0:d} - Strategy {1}", time, searchCampaignId);
                            progress.DuplicateCount++;
                        }
                        else
                        {
                            entry.State = EntityState.Detached;
                            AutoMapper.Mapper.Map(source, target);
                            entry.State = EntityState.Modified;
                            progress.UpdatedCount++;
                        }
                    }

                    progress.ItemCount++;
                }

                SafeContextWrapper.TrySaveChanges(db);
            }

            Logger.Info($"Saving {progress.ItemCount} SearchDailySummaries ({progress.UpdatedCount} updates, {progress.AddedCount} additions, {progress.DuplicateCount} duplicates)");
            return progress.ItemCount;
        }

        public static void AddUpdateDependentSearchAccounts(List<Dictionary<string, string>> items, int searchAccountId)
        {
            using (var db = new ClientPortalSearchContext())
            {
                var searchAccount = db.SearchAccounts.Find(searchAccountId);

                var accountTuples = items.Select(i => Tuple.Create(i["AccountName"], i["AccountId"])).Distinct();
                var multipleAccounts = accountTuples.Count() > 1;

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
                        existing = searchAccount.SearchProfile.SearchAccounts.SingleOrDefault(sa => sa.AccountCode == accountCode && sa.Channel == BingChannel);
                        if (existing == null)
                            existing = searchAccount.SearchProfile.SearchAccounts.SingleOrDefault(sa => sa.Name == accountName && sa.Channel == BingChannel);
                    }

                    if (existing == null)
                    {
                        searchAccount.SearchProfile.SearchAccounts.Add(new SearchAccount
                        {
                            Name = accountName,
                            Channel = BingChannel,
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

                SafeContextWrapper.TrySaveChanges(db);
            }
        }

        public static void AddUpdateDependentSearchCampaigns(List<Dictionary<string, string>> items, int searchAccountId)
        {
            bool itemsHaveAccountId = items[0].ContainsKey("AccountId");

            using (var db = new ClientPortalSearchContext())
            {
                var passedInAccount = db.SearchAccounts.Find(searchAccountId);

                IEnumerable<CampaignInfo> infos;
                if (itemsHaveAccountId)
                    infos = items.Select(c => new CampaignInfo { CampaignName = c["CampaignName"], CampaignId = c["CampaignId"], AccountId = c["AccountId"] }).Distinct();
                else
                    infos = items.Select(c => new CampaignInfo { CampaignName = c["CampaignName"], CampaignId = c["CampaignId"] }).Distinct();
                foreach (var info in infos)
                {
                    var campaignId = info.CampaignId;

                    var searchAccount = passedInAccount;
                    if (itemsHaveAccountId && searchAccount.AccountCode != info.AccountId)
                    {
                        searchAccount = searchAccount.SearchProfile.SearchAccounts.Single(sa => sa.AccountCode == info.AccountId && sa.Channel == BingChannel);
                    }
                    var existing = searchAccount.SearchCampaigns.SingleOrDefault(c => c.ExternalId == campaignId);

                    if (existing == null)
                    {
                        searchAccount.SearchCampaigns.Add(new SearchCampaign
                        {
                            SearchCampaignName = info.CampaignName,
                            ExternalId = campaignId,
                        });
                        Logger.Info("Saving new SearchCampaign: {0} ({1})", info.CampaignName, campaignId);
                        db.SaveChanges();
                    }
                    else if (existing.SearchCampaignName != info.CampaignName)
                    {
                        existing.SearchCampaignName = info.CampaignName;
                        Logger.Info("Saving updated SearchCampaign name: {0} ({1})", info.CampaignName, campaignId);
                        db.SaveChanges();
                    }
                }

                SafeContextWrapper.TrySaveChanges(db);
            }
        }
    }

    class CampaignInfo
    {
        public string AccountId { get; set; }
        public string CampaignName { get; set; }
        public string CampaignId { get; set; }
    }
}
