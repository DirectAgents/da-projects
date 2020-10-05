using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.AdWords.Exceptions;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPSearch;

namespace CakeExtracter.Etl.AdWords.Loaders
{
    internal abstract class AdWordsBaseApiLoader : Loader<Dictionary<string, string>>
    {
        protected readonly int searchAccountId;

        /// <summary>
        /// Action for exception of failed loading.
        /// </summary>
        public event Action<AdwordsFailedEtlException> ProcessFailedLoading;

        /// <summary>
        /// Gets exact extracted stats type.
        /// </summary>
        protected abstract string StatsType { get; }


        public AdWordsBaseApiLoader(int searchAccountId)
        {
            this.searchAccountId = searchAccountId;
        }

        protected override int Load(List<Dictionary<string, string>> items)
        {
            Logger.Info($"Loading {items.Count} {GetType().Name} for account (ID = {searchAccountId})..");
            EnsureDependentdData(items);
            return UpsertSummaryItems(items);
        }

        protected virtual void EnsureDependentdData(List<Dictionary<string, string>> items)
        {
            UpdateDependentSearchAccount(items);
            UpdateDependentSearchCampaigns(items);
        }

        protected abstract int UpsertSummaryItems(List<Dictionary<string, string>> items);

        protected void ProcessFailedStatsLoading(Exception e, Dictionary<string, string> item, SearchAccount account)
        {
            Logger.Warn($"Failed to load {StatsType} summary for customer ID [{item["customerID"]}] ({item["day"]}) - {e.Message}");
            var exception = GetFailedStatsLoadingException(e, item, account);
            ProcessFailedLoading?.Invoke(exception);
        }

        protected static string NetworkStringToLetter(string network)
        {
            if (string.Equals(network, "YouTube Videos", StringComparison.InvariantCultureIgnoreCase))
            {
                return "V";
            }
            return network?.Substring(0, 1);
        }

        protected static string DeviceStringToLetter(string device)
        {
            return string.IsNullOrEmpty(device) ? string.Empty : device.Substring(0, 1);
        }

        protected static string ClickTypeStringToLetter(string clickType)
        {
            if (string.Equals(clickType, "product listing ad - coupon", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Q";
            }
            else if (string.Equals(clickType, "phone calls", StringComparison.InvariantCultureIgnoreCase))
            {
                return "C";
            }

            return clickType.Substring(0, 1);
        }

        private void UpdateDependentSearchAccount(List<Dictionary<string, string>> items)
        {
            using (var db = new ClientPortalSearchContext())
            {
                var searchAccount = db.SearchAccounts.Find(searchAccountId);
                var (accountName, customerId) = items.Select(i => (i["account"], i["customerID"])).FirstOrDefault();

                if (searchAccount.Name != accountName)
                {
                    searchAccount.Name = accountName;
                    Logger.Info("Saving updated SearchAccount name: {0} ({1})", searchAccount.Name, searchAccount.SearchAccountId);
                }
                if (searchAccount.ExternalId != customerId)
                {
                    searchAccount.ExternalId = customerId;
                    Logger.Info("Saving updated SearchAccount ExternalId: {0} ({1})", searchAccount.ExternalId, searchAccount.SearchAccountId);
                }
                db.SaveChanges();
            }
        }

        private void UpdateDependentSearchCampaigns(List<Dictionary<string, string>> items)
        {
            var campaignTuples = items.Select(c => (customerId: c["customerID"], campaignName: c["campaign"], campaignId: c["campaignID"])).Distinct();
            foreach (var campaignTuple in campaignTuples)
            {
                UpsertSearchCampaign(campaignTuple);
            }
        }

        private void UpsertSearchCampaign((string customerId, string campaignName, string campaignId) campaignTuple)
        {
            using (var db = new ClientPortalSearchContext())
            {
                try
                {
                    var searchAccount = db.SearchAccounts.Find(searchAccountId);
                    var existingCampaign = searchAccount.SearchCampaigns.FirstOrDefault(c => c.ExternalId == campaignTuple.campaignId);
                    if (existingCampaign == null)
                    {
                        var campaign = new SearchCampaign
                        {
                            SearchCampaignName = campaignTuple.campaignName,
                            ExternalId = campaignTuple.campaignId,
                        };
                        searchAccount?.SearchCampaigns.Add(campaign);
                        Logger.Info("Saving new SearchCampaign: {0} ({1})", campaignTuple.campaignName, campaignTuple.campaignId);
                    }
                    else if (existingCampaign.SearchCampaignName != campaignTuple.campaignName)
                    {
                        existingCampaign.SearchCampaignName = campaignTuple.campaignName;
                        Logger.Info("Saving updated SearchCampaign name: {0} ({1})", campaignTuple.campaignName, campaignTuple.campaignId);
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.Warn($"Failed to add or update search campaign: {campaignTuple} - {ex.Message}");
                }
            }
        }

        private AdwordsFailedEtlException GetFailedStatsLoadingException(Exception e, Dictionary<string, string> item, SearchAccount account)
        {
            var fromDate = DateTime.Parse(item["day"].Replace('-', '/'));
            var toDate = DateTime.Parse(item["day"].Replace('-', '/'));
            var exception = new AdwordsFailedEtlException(fromDate, toDate, account.AccountCode, StatsType, e);
            return exception;
        }
    }
}
