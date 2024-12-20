﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CakeExtracter.Helpers;
using ClientPortal.Data.Contexts;
using DirectAgents.Domain.Contexts;

namespace CakeExtracter.Etl.SearchMarketing.Loaders
{
    public class AdWordsConvSummaryLoader : ConvSummaryLoaderBase
    {
        private readonly int searchAccountId;

        private Dictionary<string, Dictionary<DateTime, decimal>> currencyMultipliers = new Dictionary<string, Dictionary<DateTime, decimal>>();

        public AdWordsConvSummaryLoader(int searchAccountId)
        {
            this.searchAccountId = searchAccountId;
        }

        protected override int Load(List<Dictionary<string, string>> items)
        {
            Logger.Info("Loading {0} SearchConvSummaries..", items.Count);
            AddUpdateDependentConvTypes(items, "conversionName");
            AdWordsApiLoader.SetCurrencyMultipliers(items, currencyMultipliers);
            AdWordsApiLoader.AddUpdateDependentSearchAccounts(items, this.searchAccountId);
            AdWordsApiLoader.AddUpdateDependentSearchCampaigns(items, this.searchAccountId);
            var count = UpsertConvSummaries(items);
            return count;
        }

        private int UpsertConvSummaries(List<Dictionary<string, string>> items)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalContext())
            {
                var searchAccount = db.SearchAccounts.Find(this.searchAccountId);
                foreach (var item in items)
                {
                    try
                    {
                        UpsertConvSummary(item, searchAccount, db, progress);
                    }
                    catch (Exception)
                    {
                        Logger.Warn($"Failed to load search conv summary for customer ID [{item["customerID"]}] ({item["day"]})");
                        progress.SkippedCount++;
                    }
                    finally
                    {
                        progress.ItemCount++;
                    }
                }
                Logger.Info($"Saving {progress.ItemCount} SearchConvSummaries ({progress.UpdatedCount} updates, {progress.AddedCount} additions, {progress.DuplicateCount} duplicates)");
                db.SaveChanges();
            }
            return progress.ItemCount;
        }

        private void UpsertConvSummary(
            Dictionary<string, string> item,
            SearchAccount searchAccount,
            ClientPortalContext db,
            LoadingProgress progress)
        {
            var customerId = item["customerID"];
            var campaignId = item["campaignID"];
            if (searchAccount.ExternalId != customerId)
            {
                searchAccount = searchAccount.SearchProfile.SearchAccounts.Single(sa => sa.ExternalId == customerId && sa.Channel == AdWordsApiLoader.GoogleChannel);
            }
            // The SearchAccount is guaranteed to be there, having run AddUpdateDependentSearchAccounts().

            string fieldConversions, fieldConVal;
            if (searchAccount.SearchProfile.UseAllConvs)
            {
                fieldConversions = "allConv";
                fieldConVal = "allConvValue";
            }
            else
            {
                fieldConversions = "conversions";
                fieldConVal = "totalConvValue";
            }

            var scs = new SearchConvSummary
            {   //TODO: use lookup for SearchCampaignId
                SearchCampaignId = searchAccount.SearchCampaigns.Single(c => c.ExternalId == campaignId).SearchCampaignId,
                Date = DateTime.Parse(item["day"].Replace('-', '/')),
                //CurrencyId = (!item.Keys.Contains("currency") || item["currency"] == "USD") ? 1 : -1, // NOTE: non USD (if exists) -1 for now
                SearchConvTypeId = convTypeIdLookupByName[item["conversionName"]],
                Network = AdWordsApiLoader.Network_StringToLetter(item["network"]),
                Device = AdWordsApiLoader.Device_StringToLetter(item["device"]),
                Conversions = double.Parse(item[fieldConversions]),
                ConVal = decimal.Parse(item[fieldConVal]),
            };

            // Adjust ConVal if there's a Currency Multiplier...
            if (item.ContainsKey("currency") && item["currency"] != "USD")
            {
                var code = item["currency"];
                var firstOfMonth = new DateTime(scs.Date.Year, scs.Date.Month, 1);
                if (currencyMultipliers.ContainsKey(code) && currencyMultipliers[code].ContainsKey(firstOfMonth))
                {
                    var toUSDmult = currencyMultipliers[code][firstOfMonth];
                    scs.ConVal = scs.ConVal * toUSDmult;
                }
                // else... there's a problem?
            }

            var target = db.SearchConvSummaries.Find(scs.SearchCampaignId, scs.Date, scs.SearchConvTypeId, scs.Network, scs.Device);
            if (target == null)
            {
                db.SearchConvSummaries.Add(scs);
                progress.AddedCount++;
            }
            else // Summary already exists; update it
            {
                var entry = db.Entry(target);
                if (entry.State == EntityState.Unchanged)
                {
                    entry.State = EntityState.Detached;
                    AutoMapper.Mapper.Map(scs, target);
                    entry.State = EntityState.Modified;
                    progress.UpdatedCount++;
                }
                else
                {
                    Logger.Warn("Encountered duplicate for {0:d} - SearchCampaignId {1}, SearchConvTypeId {2}, Network {3}, Device {4}",
                        scs.Date, scs.SearchCampaignId, scs.SearchConvTypeId, scs.Network, scs.Device);
                    progress.DuplicateCount++;
                }
            }
        }
    }

    public abstract class ConvSummaryLoaderBase : Loader<Dictionary<string, string>>
    {
        //TODO: searchCampaignIdLookupByExternalId

        protected Dictionary<string, int> convTypeIdLookupByName = new Dictionary<string, int>();

        protected void AddUpdateDependentConvTypes(List<Dictionary<string, string>> items, string convTypePropName)
        {
            using (var db = new ClientPortalSearchContext())
            {
                var convTypeNames = items.Select(i => i[convTypePropName]).Distinct(); //TODO? make lower?
                foreach (var convTypeName in convTypeNames)
                {
                    if (convTypeIdLookupByName.ContainsKey(convTypeName))
                        continue; // already encountered this convType

                    // See if convType is in db
                    var convTypesInDb = db.SearchConvTypes.Where(s => s.Name == convTypeName);

                    if (!convTypesInDb.Any())
                    {   // SearchConvType doesn't exist in the db; so create it and put an entry in the lookup
                        var searchConvType = new DirectAgents.Domain.Entities.CPSearch.SearchConvType
                        {
                            Name = convTypeName,
                            Alias = convTypeName
                        };
                        db.SearchConvTypes.Add(searchConvType);
                        db.SaveChanges();
                        Logger.Info("Saved new SearchConvType - Name+Alias: {0} ({1})", convTypeName, searchConvType.SearchConvTypeId);
                        convTypeIdLookupByName[convTypeName] = searchConvType.SearchConvTypeId;
                    }
                    else
                    {
                        // There should be only one matching in the db. Regardless, put an entry in the lookup
                        convTypeIdLookupByName[convTypeName] = convTypesInDb.First().SearchConvTypeId;
                    }
                }
            }
        }
    }
}
