using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPSearch;

namespace CakeExtracter.Etl.AdWords.Loaders
{
    internal class AdWordsConversionTypeLoader : AdWordsBaseApiLoader
    {
        private readonly Dictionary<string, int> convTypeIdLookupByName = new Dictionary<string, int>();

        public AdWordsConversionTypeLoader(int searchAccountId)
            : base(searchAccountId)
        {
        }

        protected override void EnsureDependentdData(List<Dictionary<string, string>> items)
        {
            AddUpdateDependentConvTypes(items, "conversionName");
            base.EnsureDependentdData(items);
        }

        protected override int UpsertSummaryItems(List<Dictionary<string, string>> items)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalSearchContext())
            {
                var searchAccount = db.SearchAccounts.Find(this.searchAccountId);
                foreach (var item in items)
                {
                    try
                    {
                        UpsertConvSummary(item, searchAccount, db, progress);
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn($"Failed to load search conv summary for customer ID [{item["customerID"]}] ({item["day"]}) - {ex}");
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
            ClientPortalSearchContext db,
            LoadingProgress progress)
        {
            var scs = CreateConversionSummary(item, searchAccount);
            var target = db.SearchConvSummaries.Find(scs.SearchCampaignId, scs.Date, scs.SearchConvTypeId, scs.Network, scs.Device);

            UpdateSummary(db, progress, scs, target);
        }

        private SearchConvSummary CreateConversionSummary(Dictionary<string, string> item, SearchAccount searchAccount)
        {
            var campaignId = item["campaignID"];

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
            {
                SearchCampaignId = searchAccount.SearchCampaigns.First(c => c.ExternalId == campaignId).SearchCampaignId,
                Date = DateTime.Parse(item["day"].Replace('-', '/')),
                SearchConvTypeId = convTypeIdLookupByName[item["conversionName"]],
                Network = NetworkStringToLetter(item["network"]),
                Device = DeviceStringToLetter(item["device"]),
                Conversions = double.Parse(item[fieldConversions]),
                ConVal = decimal.Parse(item[fieldConVal]),
            };
            return scs;
        }

        private void AddUpdateDependentConvTypes(List<Dictionary<string, string>> items, string convTypePropName)
        {
            using (var db = new ClientPortalSearchContext())
            {
                var convTypeNames = items.Select(i => i[convTypePropName]).Distinct();
                foreach (var convTypeName in convTypeNames)
                {
                    if (convTypeIdLookupByName.ContainsKey(convTypeName))
                    {
                        continue;
                    }

                    var convTypesInDb = db.SearchConvTypes.Where(s => s.Name == convTypeName);

                    if (!convTypesInDb.Any())
                    {
                        var searchConvType = new SearchConvType
                        {
                            Name = convTypeName,
                            Alias = convTypeName,
                        };
                        db.SearchConvTypes.Add(searchConvType);
                        db.SaveChanges();
                        Logger.Info("Saved new SearchConvType - Name+Alias: {0} ({1})", convTypeName, searchConvType.SearchConvTypeId);
                        convTypeIdLookupByName[convTypeName] = searchConvType.SearchConvTypeId;
                    }
                    else
                    {
                        convTypeIdLookupByName[convTypeName] = convTypesInDb.First().SearchConvTypeId;
                    }
                }
            }
        }

        private static void UpdateSummary(ClientPortalSearchContext db, LoadingProgress progress, SearchConvSummary scs, SearchConvSummary target)
        {
            if (target == null)
            {
                db.SearchConvSummaries.Add(scs);
                progress.AddedCount++;
            }
            else
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
                    Logger.Warn("Encountered duplicate for {0:d} - SearchCampaignId {1}, SearchConvTypeId {2}, Network {3}, Device {4}", scs.Date, scs.SearchCampaignId, scs.SearchConvTypeId, scs.Network, scs.Device);
                    progress.DuplicateCount++;
                }
            }
        }
    }
}
