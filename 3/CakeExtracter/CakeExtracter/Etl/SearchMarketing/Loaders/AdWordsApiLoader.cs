﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CakeExtracter.Helpers;
using ClientPortal.Data.Contexts;

using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPSearch;

using SearchAccount = ClientPortal.Data.Contexts.SearchAccount;
using SearchCampaign = ClientPortal.Data.Contexts.SearchCampaign;
using SearchDailySummary = ClientPortal.Data.Contexts.SearchDailySummary;

namespace CakeExtracter.Etl.SearchMarketing.Loaders
{
    public class AdWordsApiLoader : Loader<Dictionary<string, string>>
    {
        public const string GoogleChannel = "Google";
        private readonly int searchAccountId;
        private readonly bool includeClickType; // if true, we use SearchDailySummary2's
        private readonly bool clickAssistConvStats;

        private Dictionary<string, Dictionary<DateTime, decimal>> currencyMultipliers = new Dictionary<string, Dictionary<DateTime, decimal>>();

        public AdWordsApiLoader(int searchAccountId, bool includeClickType, bool clickAssistConvStats = false)
        {
            this.searchAccountId = searchAccountId;
            this.includeClickType = includeClickType;
            this.clickAssistConvStats = clickAssistConvStats;
        }

        protected override int Load(List<Dictionary<string, string>> items)
        {
            Logger.Info($"Loading {items.Count} SearchDailySummaries for account (ID = {searchAccountId})..");
            SetCurrencyMultipliers(items, this.currencyMultipliers);
            AddUpdateDependentSearchAccounts(items, this.searchAccountId);
            AddUpdateDependentSearchCampaigns(items, this.searchAccountId);
            var count = UpsertSearchDailySummaries(items);
            if (!clickAssistConvStats && !includeClickType)
            {
                UpsertSearchVideoDailySummaries(items);
            }

            return count;
        }

        public static string Network_StringToLetter(string network)
        {
            switch (network) {
                case null:
                    return null;
                case "YouTube Videos":
                    return "V"; // Y will be for "YouTube Search"
                default:
                    return network.Substring(0, 1);
            }
        }

        public static string Device_StringToLetter(string device)
        {
            return string.IsNullOrEmpty(device) ? string.Empty : device.Substring(0, 1);
        }

        public static string ClickType_StringToLetter(string clickType)
        {
            var clickTypeAbbrev = clickType.Substring(0, 1);
            var ctLower = clickType.ToLower();
            if (ctLower == "product listing ad - coupon") // started on 10/18/14 for Folica|Search (conflict with "Product listing ad")
                clickTypeAbbrev = "Q";
            else if (ctLower == "phone calls") // noticed for The Credit Pros -> "DA Spanish Mobile" on 11/11/17
                clickTypeAbbrev = "C";
            return clickTypeAbbrev;
        }

        private int UpsertSearchDailySummaries(List<Dictionary<string, string>> items)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalContext())
            {
                var searchAccount = db.SearchAccounts.Find(this.searchAccountId);
                foreach (var item in items)
                {
                    try
                    {
                        UpsertSearchDailySummary(item, searchAccount, db, progress);
                    }
                    catch (Exception)
                    {
                        Logger.Warn($"Failed to load search daily summary for customer ID [{item["customerID"]}] ({item["day"]})");
                        progress.SkippedCount++;
                    }
                    finally
                    {
                        progress.ItemCount++;
                    }
                }
                Logger.Info($"Saving {progress.ItemCount} SearchDailySummaries ({progress.UpdatedCount} updates, {progress.AddedCount} additions)");
                db.SaveChanges();
            }
            return progress.ItemCount;
        }

        private int UpsertSearchVideoDailySummaries(List<Dictionary<string, string>> items)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalSearchContext())
            {
                var searchAccount = db.SearchAccounts.Find(this.searchAccountId);
                foreach (var item in items)
                {
                    try
                    {
                        UpsertSearchVideoDailySummary(item, searchAccount, db, progress);
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn($"Failed to load search daily summary for customer ID [{item["customerID"]}] ({item["day"]})");
                        progress.SkippedCount++;
                    }
                    finally
                    {
                        progress.ItemCount++;
                    }
                }
                Logger.Info($"Saving {progress.ItemCount} SearchVideoDailySummaries ({progress.UpdatedCount} updates, {progress.AddedCount} additions)");
                db.SaveChanges();
            }
            return progress.ItemCount;
        }

        private void UpsertSearchDailySummary(
            Dictionary<string, string> item,
            SearchAccount searchAccount,
            ClientPortalContext db,
            LoadingProgress progress)
        {
            var customerId = item["customerID"];
            var campaignId = item["campaignID"];
            if (searchAccount.ExternalId != customerId)
            {
                searchAccount = searchAccount.SearchProfile.SearchAccounts.Single(sa => sa.ExternalId == customerId && sa.Channel == GoogleChannel);
            }
            var searchDailySummary = CreateBaseSearchDailySummary(item, searchAccount, campaignId);
            if (clickAssistConvStats)
            {
                SetFieldsForClickAssistConvSummary(searchDailySummary, item);
            }
            else
            {
                SetFieldsForSummary(searchDailySummary, item, searchAccount);
            }
            SetCurrencyForSummary(searchDailySummary, item);
            var isAdded = includeClickType
                ? UpsertSearchDailySummary(db, searchDailySummary, item["clickType"])
                : UpsertSearchDailySummary(db, searchDailySummary);
            if (isAdded)
            {
                progress.AddedCount++;
            }
            else
            {
                progress.UpdatedCount++;
            }
        }

        private void UpsertSearchVideoDailySummary(
            Dictionary<string, string> item,
            DirectAgents.Domain.Entities.CPSearch.SearchAccount searchAccount,
            ClientPortalSearchContext db,
            LoadingProgress progress)
        {
            var customerId = item["customerID"];
            var campaignId = item["campaignID"];
            if (searchAccount.ExternalId != customerId)
            {
                searchAccount = searchAccount.SearchProfile.SearchAccounts.Single(sa => sa.ExternalId == customerId && sa.Channel == GoogleChannel);
            }
            var searchVideoDailySummary = CreateSearchVideoDailySummary(item, searchAccount, campaignId);

            var isAdded = UpsertSearchVideoDailySummary(db, searchVideoDailySummary);
            if (isAdded)
            {
                progress.AddedCount++;
            }
            else
            {
                progress.UpdatedCount++;
            }
        }

        private SearchDailySummary CreateBaseSearchDailySummary(
            Dictionary<string, string> item,
            SearchAccount searchAccount,
            string campaignId)
        {
            var searchCampaignId = searchAccount.SearchCampaigns.Single(c => c.ExternalId == campaignId).SearchCampaignId;
            var date = DateTime.Parse(item["day"].Replace('-', '/'));
            var currencyId = (!item.Keys.Contains("currency") || item["currency"] == "USD") ? 1 : -1; // NOTE: non USD (if exists) -1 for now
            var network = Network_StringToLetter(item["network"]);
            return new SearchDailySummary
            {
                // the basic fields
                SearchCampaignId = searchCampaignId,
                Date = date,
                CurrencyId = currencyId,
                Network = network,
            };
        }

        private SearchVideoDailySummary CreateSearchVideoDailySummary(
            IReadOnlyDictionary<string, string> item,
            DirectAgents.Domain.Entities.CPSearch.SearchAccount searchAccount,
            string campaignId)
        {
            var searchCampaignId = searchAccount.SearchCampaigns.Single(c => c.ExternalId == campaignId).SearchCampaignId;
            var date = DateTime.Parse(item["day"].Replace('-', '/'));
            return new SearchVideoDailySummary
                   {
                       SearchCampaignId = searchCampaignId,
                       Date = date,
                       Device = Device_StringToLetter(item["device"]),
                       Network = Network_StringToLetter(item["network"]),
                       VideoPlayedTo25 = double.Parse(item["videoPlayedTo25"].Replace("%", string.Empty)),
                       VideoPlayedTo50 = double.Parse(item["videoPlayedTo50"].Replace("%", string.Empty)),
                       VideoPlayedTo75 = double.Parse(item["videoPlayedTo75"].Replace("%", string.Empty)),
                       VideoPlayedTo100 = double.Parse(item["videoPlayedTo100"].Replace("%", string.Empty)),
                       Views = int.Parse(item["views"]),
                       ActiveViewImpressions = int.Parse(item["activeViewViewableImpressions"]),
                   };
        }

        private void SetFieldsForClickAssistConvSummary(SearchDailySummary summary, Dictionary<string, string> item)
        {
            summary.Device = "A"; // mark for "all" devices
            summary.CassConvs = int.Parse(item["clickAssistedConv"]);
            summary.CassConVal = double.TryParse(item["clickAssistedConvValue"], out var cassConVal) ? cassConVal : 0.0;
        }

        private void SetFieldsForSummary(
            SearchDailySummary summary,
            Dictionary<string, string> item,
            SearchAccount searchAccount)
        {
            summary.Revenue = decimal.Parse(item["totalConvValue"]);
            summary.Cost = decimal.Parse(item["cost"]) / 1000000; // convert from microns to dollars
            var conversions = double.Parse(item["conversions"]);
            summary.Orders = Convert.ToInt32(conversions); // default rounding - nearest even # if .5
            summary.Clicks = int.Parse(item["clicks"]);
            summary.Impressions = int.Parse(item["impressions"]);
            summary.Device = Device_StringToLetter(item["device"]);
            summary.ViewThrus = int.Parse(item["viewThroughConv"]);

            if (searchAccount.RevPerOrder.HasValue)
            {
                summary.Revenue = summary.Orders * searchAccount.RevPerOrder.Value;
            }
        }

        private void SetCurrencyForSummary(SearchDailySummary summary, Dictionary<string, string> item)
        {
            // Adjust revenue and cost if there's a Currency Multiplier...
            if (!item.Keys.Contains("currency") || item["currency"] == "USD")
            {
                summary.CurrencyId = 1; // USD or not specified
            }
            else
            {
                var code = item["currency"];
                var firstOfMonth = new DateTime(summary.Date.Year, summary.Date.Month, 1);
                if (!currencyMultipliers.ContainsKey(code) || !currencyMultipliers[code].ContainsKey(firstOfMonth))
                {
                    summary.CurrencyId = -1;
                }
                else
                {
                    summary.CurrencyId = 1;
                    var toUSDmult = currencyMultipliers[code][firstOfMonth];
                    summary.Revenue = summary.Revenue * toUSDmult;
                    summary.Cost = summary.Cost * toUSDmult;

                    // TODO? Do for CassConVal as well?
                }
            }
        }

        // return true if added; false if updated
        private bool UpsertSearchDailySummary(ClientPortalContext db, SearchDailySummary sds)
        {
            var target = db.Set<SearchDailySummary>().Find(sds.SearchCampaignId, sds.Date, sds.Network, sds.Device);
            if (target == null)
            {
                db.SearchDailySummaries.Add(sds);
                return true;
            }
            else
            {
                db.Entry(target).State = EntityState.Detached;

                // For Megabus conversions fix. Also comment out sds.Cost/Clicks/Impressions assignments above.
                // sds.Impressions = target.Impressions;
                // sds.Clicks = target.Clicks;
                // sds.Cost = target.Cost;
                target = AutoMapper.Mapper.Map(sds, target);
                db.Entry(target).State = EntityState.Modified;
                return false;
            }
        }

        private bool UpsertSearchVideoDailySummary(ClientPortalSearchContext db, SearchVideoDailySummary sds)
        {
            var target = db.Set<SearchVideoDailySummary>().Find(sds.SearchCampaignId, sds.Date, sds.Network, sds.Device);
            if (target == null)
            {
                db.SearchVideoDailySummaries.Add(sds);
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

        private bool UpsertSearchDailySummary(ClientPortalContext db, SearchDailySummary sds, string clickType)
        {
            var clickTypeAbbrev = ClickType_StringToLetter(clickType);
            var sds2 = new SearchDailySummary2
            {
                SearchCampaignId = sds.SearchCampaignId,
                Date = sds.Date,
                Network = sds.Network,
                Device = sds.Device,
                ClickType = clickTypeAbbrev,
                Revenue = sds.Revenue,
                Cost = sds.Cost,
                Orders = sds.Orders,
                Clicks = sds.Clicks,
                // HACK: ignoring impressions for rows that do not have H or C as click type
                Impressions = (clickTypeAbbrev == "H" || clickTypeAbbrev == "C") ? sds.Impressions : 0,
                CurrencyId = sds.CurrencyId,
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

        public static void SetCurrencyMultipliers(List<Dictionary<string, string>> items,
            Dictionary<string, Dictionary<DateTime, decimal>> currencyMultipliers)
        {
            if (!items.Any() || !items[0].Keys.Contains("currency"))
            {
                return;
            }

            var currencyTuples1 = items.Where(i => i["currency"] != "USD")
                .Select(i => Tuple.Create(i["currency"], DateTime.Parse(i["day"].Replace('-', '/')))).Distinct();
            var currencyTuples = currencyTuples1
                .Select(t => Tuple.Create(t.Item1, new DateTime(t.Item2.Year, t.Item2.Month, 1))).Distinct().ToList();
            var currencyGroups = currencyTuples.GroupBy(ct => ct.Item1); // group by currency code

            foreach (var currencyGroup in currencyGroups) // (foreach currency code)
            {
                if (currencyMultipliers.ContainsKey(currencyGroup.Key))
                {
                    // See if dictionary values are already set
                    var monthsWithoutMultiplier =
                        currencyGroup.Where(t => !currencyMultipliers[currencyGroup.Key].ContainsKey(t.Item2));
                    if (!monthsWithoutMultiplier.Any())
                    {
                        continue; // they're all there already
                    }
                }
                else
                {
                    currencyMultipliers.Add(currencyGroup.Key, new Dictionary<DateTime, decimal>());
                }

                SetCurrencyMultiplier(currencyGroup, currencyMultipliers);
            }
        }

        public static void AddUpdateDependentSearchAccounts(List<Dictionary<string, string>> items, int searchAccountId)
        {
            using (var db = new ClientPortalContext())
            {
                var searchAccount = db.SearchAccounts.Find(searchAccountId);

                var accountTuples = items.Select(i => Tuple.Create(i["account"], i["customerID"])).Distinct().ToList();
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
                        existing = searchAccount.SearchProfile.SearchAccounts.SingleOrDefault(sa => sa.ExternalId == customerId && sa.Channel == GoogleChannel);
                        if (existing == null)
                            existing = searchAccount.SearchProfile.SearchAccounts.SingleOrDefault(sa => sa.Name == accountName && sa.Channel == GoogleChannel);
                    }

                    if (existing == null)
                    {
                        searchAccount.SearchProfile.SearchAccounts.Add(new SearchAccount
                        {
                            Name = accountName,
                            Channel = GoogleChannel,
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

        public static void AddUpdateDependentSearchCampaigns(List<Dictionary<string, string>> items, int searchAccountId)
        {
            var campaignTuples = items.Select(c => Tuple.Create(c["customerID"], c["campaign"], c["campaignID"])).Distinct();
            using (var db = new ClientPortalContext())
            {
                var searchAccount = db.SearchAccounts.Find(searchAccountId);
                foreach (var campaignTuple in campaignTuples)
                {
                    try
                    {
                        AddUpdateDependentSearchCampaign(campaignTuple, searchAccount, db);
                    }
                    catch (Exception)
                    {
                        Logger.Warn($"Failed to add or update search campaign: {campaignTuple}");
                    }
                }
            }
        }

        private static void AddUpdateDependentSearchCampaign(
            Tuple<string, string, string> campaignTuple,
            SearchAccount searchAccount,
            ClientPortalContext db)
        {
            var customerId = campaignTuple.Item1;
            var campaignName = campaignTuple.Item2;
            var campaignId = campaignTuple.Item3;
            if (searchAccount?.ExternalId != customerId)
            {
                searchAccount = searchAccount?.SearchProfile.SearchAccounts.Single(sa =>
                    sa.ExternalId == customerId && sa.Channel == GoogleChannel);
            }

            var existingCampaign =
                searchAccount?.SearchCampaigns.SingleOrDefault(c => c.ExternalId == campaignId);
            if (existingCampaign == null)
            {
                var campaign = new SearchCampaign
                {
                    SearchCampaignName = campaignName,
                    ExternalId = campaignId,
                };
                searchAccount?.SearchCampaigns.Add(campaign);
                Logger.Info("Saving new SearchCampaign: {0} ({1})", campaignName, campaignId);
                db.SaveChanges();
            }
            else if (existingCampaign.SearchCampaignName != campaignName)
            {
                existingCampaign.SearchCampaignName = campaignName;
                Logger.Info("Saving updated SearchCampaign name: {0} ({1})", campaignName, campaignId);
                db.SaveChanges();
            }
        }

        private static void SetCurrencyMultiplier(IGrouping<string, Tuple<string, DateTime>> currencyGroup,
            Dictionary<string, Dictionary<DateTime, decimal>> currencyMultipliers)
        {
            try
            {
                var singleCurrencyMultipliers = currencyMultipliers[currencyGroup.Key];
                using (var db = new ClientPortalContext())
                {
                    var currConversions = GetCurrencyConversions(currencyGroup.Key, db);

                    if (!currConversions.Any())
                        return;

                    var earliestConvDate = currConversions.First().Date;
                    foreach (var currTuple in currencyGroup) // (foreach month)
                    {
                        if (singleCurrencyMultipliers.ContainsKey(currTuple.Item2))
                            continue;

                        if (currTuple.Item2 < earliestConvDate)
                        {
                            // Use the oldest CurrencyConversion
                            singleCurrencyMultipliers.Add(currTuple.Item2, currConversions.First().ToUSDmultiplier);
                        }
                        else
                        {
                            // Use the most recent CurrencyConversion that's <= the currTuple's date
                            var currConv = currConversions.Where(c => c.Date <= currTuple.Item2)
                                .OrderBy(c => c.Date).Last();
                            singleCurrencyMultipliers.Add(currTuple.Item2, currConv.ToUSDmultiplier);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(new Exception($"Could not set currency multiplier [{currencyGroup.Key}]", e));
            }
        }

        private static List<CurrencyConversion> GetCurrencyConversions(string currencyGroupKey, ClientPortalContext db)
        {
            var currencyConversions = new List<CurrencyConversion>();
            try
            {
                currencyConversions = db.CurrencyConversions.Where(c => c.Currency.Code == currencyGroupKey)
                    .OrderBy(c => c.Date).ToList();
                return currencyConversions;
            }
            catch (Exception e)
            {
                if (e.InnerException is System.Data.SqlClient.SqlException)
                {
                    Logger.Warn("Table 'dbo.CurrencyConversion' does not exist in database");
                    return currencyConversions;
                }

                throw new Exception("Could not get a list of currency conversions.", e);
            }
        }
    }
}
