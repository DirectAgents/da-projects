using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Apple;
using CakeExtracter.Etl.Apple.Exceptions;
using ClientPortal.Data.Contexts;

namespace CakeExtracter.Etl.Apple.Loaders
{
    public class AppleApiLoader : Loader<AppleStatGroup>
    {
        public const string AppleChannel = "Apple";
        private readonly SearchAccount account;

        public event Action<AppleFailedEtlException> ProcessFailedLoading;

        private Dictionary<string, int> campaignIdLookupByExtIdString = new Dictionary<string, int>();

        public AppleApiLoader(SearchAccount searchAccount)
        {
            this.BatchSize = AppleAdsUtility.RowsReturnedAtATime;
            this.account = searchAccount;
        }

        protected override int Load(List<AppleStatGroup> items)
        {
            try
            {
                Logger.Info("Loading {0} AppleStatGroups..", items.Count);
                AddUpdateDependentSearchCampaigns(items);
                UpsertSearchDailySummaries(items);
                return items.Count;
            }
            catch (Exception e)
            {
                ProcessFailedStatsLoading(e, items);
                return items.Count;
            }
        }

        protected virtual AppleFailedEtlException GetFailedStatsLoadingException(Exception e, List<AppleStatGroup> items)
        {
            var fromDate = items.Min(x => x.granularity.Min(y => y.Date));
            var toDate = items.Max(x => x.granularity.Max(y => y.Date));
            var fromDateArg = fromDate == default(DateTime) ? null : (DateTime?)fromDate;
            var toDateArg = toDate == default(DateTime) ? null : (DateTime?)toDate;
            var exception = new AppleFailedEtlException(fromDateArg, toDateArg, account.AccountCode, e);
            return exception;
        }

        private int UpsertSearchDailySummaries(List<AppleStatGroup> statGroups)
        {
            var addedCount = 0;
            var updatedCount = 0;
            var itemCount = 0;
            using (var db = new ClientPortalContext())
            {
                foreach (var statGroup in statGroups)
                {
                    var metadata = statGroup.metadata;
                    if (!campaignIdLookupByExtIdString.ContainsKey(metadata.campaignId))
                    {
                        Logger.Warn("Skipping StatGroup. No campaign for ({0})", metadata.campaignId);
                        continue;
                    }
                    int searchCampaignId = campaignIdLookupByExtIdString[metadata.campaignId];

                    foreach (var appleStat in statGroup.granularity)
                    {
                        var pk1 = searchCampaignId;
                        var pk2 = appleStat.Date;
                        var pk3 = ".";
                        var pk4 = ".";
                        var source = new SearchDailySummary
                        {
                            SearchCampaignId = pk1,
                            Date = pk2,
                            Network = pk3,
                            Device = pk4,
                            //Revenue =
                            Cost = appleStat.LocalSpend.amount,
                            Orders = appleStat.Installs,
                            Clicks = appleStat.Taps,
                            Impressions = appleStat.Impressions,
                            CurrencyId = 1 // appleStat.localSpend.currency == "USD" ? 1 : -1 // NOTE: non USD (if exists) -1 for now
                        };
                        var target = db.Set<SearchDailySummary>().Find(pk1, pk2, pk3, pk4);
                        if (target == null)
                        {
                            db.SearchDailySummaries.Add(source);
                            addedCount++;
                        }
                        else
                        {
                            var entry = db.Entry(target);
                            entry.State = EntityState.Detached;
                            AutoMapper.Mapper.Map(source, target);
                            entry.State = EntityState.Modified;
                            updatedCount++;
                        }
                        itemCount++;
                    }
                }
                Logger.Info("Saving {0} SearchDailySummaries ({1} updates, {2} additions)", itemCount, updatedCount, addedCount);
                db.SaveChanges();
            }
            return 0; // which count to return?
        }

        public void AddUpdateDependentSearchCampaigns(List<AppleStatGroup> statGroups)
        {
            using (var db = new ClientPortalContext())
            {
                var searchAccount = db.SearchAccounts.Find(account.SearchAccountId);
                foreach (var statGroup in statGroups)
                {
                    if (statGroup.metadata == null)
                        continue;
                    var metadata = statGroup.metadata;

                    int campaignId;
                    if (!int.TryParse(metadata.campaignId, out campaignId))
                    {
                        Logger.Warn("campaignId ({0}) is not an int", campaignId);
                        continue;
                    }
                    var existing = searchAccount.SearchCampaigns.SingleOrDefault(c => c.ExternalId == campaignId);
                    if (existing == null)
                    {
                        var searchCampaign = new SearchCampaign
                        {
                            SearchCampaignName = metadata.campaignName,
                            ExternalId = campaignId
                        };
                        searchAccount.SearchCampaigns.Add(searchCampaign);
                        Logger.Info("Saving new SearchCampaign: {0} ({1})", metadata.campaignName, campaignId);
                        db.SaveChanges();
                        campaignIdLookupByExtIdString[metadata.campaignId] = searchCampaign.SearchCampaignId;
                    }
                    else
                    {
                        if (existing.SearchCampaignName != metadata.campaignName)
                        {
                            existing.SearchCampaignName = metadata.campaignName;
                            Logger.Info("Saving updated SearchCampaign name: {0} ({1})", metadata.campaignName, campaignId);
                            db.SaveChanges();
                        }
                        campaignIdLookupByExtIdString[metadata.campaignId] = existing.SearchCampaignId;
                    }
                }
            }
        }

        private void ProcessFailedStatsLoading(Exception e, List<AppleStatGroup> items)
        {
            Logger.Error(e);
            var exception = GetFailedStatsLoadingException(e, items);
            ProcessFailedLoading?.Invoke(exception);
        }
    }
}
