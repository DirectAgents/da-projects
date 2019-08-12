using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Apple;
using CakeExtracter.Etl.Apple.Exceptions;
using CakeExtracter.Helpers;
using ClientPortal.Data.Contexts;

namespace CakeExtracter.Etl.Apple.Loaders
{
    /// <summary>
    /// Apple daily Ads stats loader.
    /// </summary>
    public class AppleApiLoader : Loader<AppleStatGroup>
    {
        private readonly SearchAccount searchAccount;

        private readonly Dictionary<string, int> campaignIdLookupByExtIdString = new Dictionary<string, int>();

        /// <summary>
        /// Action for exception of failed loading.
        /// </summary>
        public event Action<AppleFailedEtlException> ProcessFailedLoading;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleApiLoader"/> class.
        /// </summary>
        /// <param name="searchAccount">Current search account.</param>
        public AppleApiLoader(SearchAccount searchAccount)
        {
            this.BatchSize = AppleAdsUtility.RowsReturnedAtATime;
            this.searchAccount = searchAccount;
        }

        /// <summary>
        /// Loads the specified items.
        /// </summary>
        /// <param name="items">List of items for loading.</param>
        /// <returns>Count of loaded items.</returns>
        protected override int Load(List<AppleStatGroup> items)
        {
            try
            {
                Logger.Info("Loading {0} AppleStatGroups..", items.Count);
                if (items.Count == 0)
                {
                    return 0;
                }
                AddUpdateDependentSearchCampaigns(items);
                var count = UpsertSearchDailySummaries(items);
                return count;
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
            var exception = new AppleFailedEtlException(fromDateArg, toDateArg, searchAccount.AccountCode, e);
            return exception;
        }

        private int UpsertSearchDailySummaries(List<AppleStatGroup> statGroups)
        {
            const string networkValue = ".";
            const string deviceValue = ".";
            var progress = new LoadingProgress();
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
                    var searchCampaignId = campaignIdLookupByExtIdString[metadata.campaignId];

                    foreach (var appleStat in statGroup.granularity)
                    {
                        var source = new SearchDailySummary
                        {
                            SearchCampaignId = searchCampaignId,
                            Date = appleStat.Date,
                            Network = networkValue,
                            Device = deviceValue,
                            Cost = appleStat.LocalSpend.amount,
                            Orders = appleStat.Installs,
                            Clicks = appleStat.Taps,
                            Impressions = appleStat.Impressions,
                            CurrencyId = 1, // appleStat.localSpend.currency == "USD" ? 1 : -1 // NOTE: non USD (if exists) -1 for now
                        };
                        var target = db.Set<SearchDailySummary>().Find(searchCampaignId, appleStat.Date, networkValue, deviceValue);
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
                                Logger.Warn("Encountered duplicate for {0:d} - Strategy {1}", appleStat.Date, searchCampaignId);
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
                }
                SafeContextWrapper.TrySaveChanges(db);
            }
            Logger.Info($"Saving {progress.ItemCount} SearchDailySummaries ({progress.UpdatedCount} updates, {progress.AddedCount} additions), {progress.DuplicateCount} duplicates)");
            return statGroups.Count;
        }

        private void AddUpdateDependentSearchCampaigns(List<AppleStatGroup> statGroups)
        {
            using (var db = new ClientPortalContext())
            {
                var existingSearchAccount = db.SearchAccounts.Find(searchAccount.SearchAccountId);
                foreach (var statGroup in statGroups)
                {
                    if (statGroup.metadata == null)
                    {
                        continue;
                    }
                    var metadata = statGroup.metadata;

                    var campaignId = metadata.campaignId;
                    if (!long.TryParse(campaignId, out _))
                    {
                        Logger.Warn($"CampaignId ({campaignId}) is not an int or long");
                        continue;
                    }
                    var existingCampaign = existingSearchAccount?.SearchCampaigns.SingleOrDefault(c => c.ExternalId == campaignId);
                    if (existingCampaign == null)
                    {
                        var searchCampaign = new SearchCampaign
                        {
                            SearchCampaignName = metadata.campaignName,
                            ExternalId = campaignId,
                        };
                        existingSearchAccount?.SearchCampaigns.Add(searchCampaign);
                        Logger.Info("Saving new SearchCampaign: {0} ({1})", metadata.campaignName, campaignId);
                        db.SaveChanges();
                        campaignIdLookupByExtIdString[metadata.campaignId] = searchCampaign.SearchCampaignId;
                    }
                    else
                    {
                        if (existingCampaign.SearchCampaignName != metadata.campaignName)
                        {
                            existingCampaign.SearchCampaignName = metadata.campaignName;
                            Logger.Info("Saving updated SearchCampaign name: {0} ({1})", metadata.campaignName, campaignId);
                            db.SaveChanges();
                        }
                        campaignIdLookupByExtIdString[metadata.campaignId] = existingCampaign.SearchCampaignId;
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
