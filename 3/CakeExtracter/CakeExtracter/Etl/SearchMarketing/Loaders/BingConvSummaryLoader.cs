using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPSearch;

namespace CakeExtracter.Etl.SearchMarketing.Loaders
{
    public class BingConvSummaryLoader : ConvSummaryLoaderBase
    {
        private readonly int searchAccountId;
        private bool hasAccountId = false;

        public BingConvSummaryLoader(int searchAccountId)
        {
            this.searchAccountId = searchAccountId;
        }

        protected override int Load(List<Dictionary<string, string>> items)
        {
            Logger.Info("Loading {0} SearchConvSummaries..", items.Count);
            if (items.Count == 0)
            {
                return 0;
            }

            AddUpdateDependentConvTypes(items, "Goal");
            hasAccountId = items[0].ContainsKey("AccountId");
            if (hasAccountId)
            {
                BingLoader.AddUpdateDependentSearchAccounts(items, this.searchAccountId);
            }
            BingLoader.AddUpdateDependentSearchCampaigns(items, this.searchAccountId);
            var count = UpsertConvSummaries(items);
            return count;
        }

        private int UpsertConvSummaries(List<Dictionary<string, string>> items)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalSearchContext())
            {
                var passedInAccount = db.SearchAccounts.Find(this.searchAccountId);

                foreach (var item in items)
                {
                    var campaignId = item["CampaignId"];

                    var searchAccount = passedInAccount;
                    if (hasAccountId)
                    {
                        var accountCode = item["AccountId"];
                        if (searchAccount.AccountCode != accountCode)
                        {
                            searchAccount = searchAccount.SearchProfile.SearchAccounts.Single(sa => sa.AccountCode == accountCode && sa.Channel == BingLoader.BingChannel);
                        }
                    }

                    var scs = new SearchConvSummary
                    {   //TODO: use lookup for SearchCampaignId
                        SearchCampaignId = searchAccount.SearchCampaigns.Single(c => c.ExternalId == campaignId).SearchCampaignId,
                        Date = DateTime.Parse(item["TimePeriod"]),
                        SearchConvTypeId = convTypeIdLookupByName[item["Goal"]],
                        Network = ".",
                        Device = ".",
                        Conversions = double.Parse(item["AllConversions"]),
                        ConVal = decimal.Parse(item["AllRevenue"]),
                        //CurrencyId
                    };

                    //TODO: Adjust ConVal if there's a Currency Multiplier... (see AdWordsConvSummaryLoader)

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
                    progress.ItemCount++;
                }
                Logger.Info($"Saving {progress.ItemCount} SearchConvSummaries ({progress.UpdatedCount} updates, {progress.AddedCount} additions, {progress.DuplicateCount} duplicates)");
                db.SaveChanges();
            }
            return progress.ItemCount;
        }
    }
}
