using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPSearch;

namespace CakeExtracter.Etl.AdWords.Loaders
{
    internal class AdWordsAssistConvLoader : AdWordsBaseApiLoader
    {
        public AdWordsAssistConvLoader(int searchAccountId)
        : base(searchAccountId)
        {
        }

        protected override int UpsertSummaryItems(List<Dictionary<string, string>> items)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalSearchContext())
            {
                var searchAccount = db.SearchAccounts.Find(searchAccountId);
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

        private void UpsertSearchDailySummary(
            Dictionary<string, string> item,
            SearchAccount searchAccount,
            ClientPortalSearchContext db,
            LoadingProgress progress)
        {
            var campaignId = item["campaignID"];
            var searchDailySummary = CreateBaseSearchDailySummary(item, searchAccount, campaignId);

            SetFieldsForClickAssistConvSummary(searchDailySummary, item);

            if (UpsertSummary(db, searchDailySummary))
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
            var currencyId = (!item.Keys.Contains("currency") || item["currency"] == "USD") ? 1 : -1;
            var network = NetworkStringToLetter(item["network"]);
            return new SearchDailySummary
            {

                SearchCampaignId = searchCampaignId,
                Date = date,
                CurrencyId = currencyId,
                Network = network,
            };
        }

        private void SetFieldsForClickAssistConvSummary(SearchDailySummary summary, Dictionary<string, string> item)
        {
            summary.Device = "A";
            summary.CassConvs = int.Parse(item["clickAssistedConv"]);
            summary.CassConVal = double.TryParse(item["clickAssistedConvValue"], out var cassConVal) ? cassConVal : 0.0;
        }

        private bool UpsertSummary<T>(ClientPortalSearchContext db, T summaryItem)
            where T : DailySummaryBase
        {
            var target = db.Set<T>().Find(summaryItem.SearchCampaignId, summaryItem.Date, summaryItem.Network, summaryItem.Device);
            if (target == null)
            {
                db.Set<T>().Add(summaryItem);
                return true;
            }
            else
            {
                db.Entry(target).State = EntityState.Detached;
                target = AutoMapper.Mapper.Map(summaryItem, target);
                db.Entry(target).State = EntityState.Modified;
                return false;
            }
        }
    }
}
