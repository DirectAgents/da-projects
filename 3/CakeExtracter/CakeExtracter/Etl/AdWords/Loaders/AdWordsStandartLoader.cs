using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPSearch;

namespace CakeExtracter.Etl.AdWords.Loaders
{
    // TODO: Create maps in automaper for the data formating logic and change legacy Upsert methods to generic.
    internal class AdWordsStandartLoader : AdWordsBaseApiLoader
    {
        public AdWordsStandartLoader(int searchAccountId)
            : base(searchAccountId)
        {
        }

        /// <inheritdoc/>
        protected override string StatsType => string.Empty;

        protected override int UpsertSummaryItems(List<Dictionary<string, string>> items)
        {
            UpsertSearchVideoDailySummaries(items);
            return UpsertSearchDailySummaries(items);
        }

        private int UpsertSearchDailySummaries(List<Dictionary<string, string>> items)
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
                    catch (Exception ex)
                    {
                        ProcessFailedStatsLoading(ex, item, searchAccount);
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
                var searchAccount = db.SearchAccounts.Find(searchAccountId);
                foreach (var item in items)
                {
                    try
                    {
                        UpsertSearchVideoDailySummary(item, searchAccount, db, progress);
                    }
                    catch (Exception ex)
                    {
                        ProcessFailedStatsLoading(ex, item, searchAccount);
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
            ClientPortalSearchContext db,
            LoadingProgress progress)
        {
            var campaignId = item["campaignID"];
            var searchDailySummary = CreateBaseSearchDailySummary(item, searchAccount, campaignId);

            SetFieldsForSummary(searchDailySummary, item, searchAccount);

            if (UpsertSummary(db, searchDailySummary))
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
            SearchAccount searchAccount,
            ClientPortalSearchContext db,
            LoadingProgress progress)
        {
            var campaignId = item["campaignID"];
            var searchVideoDailySummary = CreateSearchVideoDailySummary(item, searchAccount, campaignId);

            if (UpsertSummary(db, searchVideoDailySummary))
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

        private SearchVideoDailySummary CreateSearchVideoDailySummary(
            Dictionary<string, string> item,
            SearchAccount searchAccount,
            string campaignId)
        {
            var searchCampaignId = searchAccount.SearchCampaigns.Single(c => c.ExternalId == campaignId).SearchCampaignId;
            var date = DateTime.Parse(item["day"].Replace('-', '/'));
            return new SearchVideoDailySummary
            {
                SearchCampaignId = searchCampaignId,
                Date = date,
                Device = DeviceStringToLetter(item["device"]),
                Network = NetworkStringToLetter(item["network"]),
                VideoPlayedTo25 = double.Parse(item["videoPlayedTo25"].Replace("%", string.Empty)),
                VideoPlayedTo50 = double.Parse(item["videoPlayedTo50"].Replace("%", string.Empty)),
                VideoPlayedTo75 = double.Parse(item["videoPlayedTo75"].Replace("%", string.Empty)),
                VideoPlayedTo100 = double.Parse(item["videoPlayedTo100"].Replace("%", string.Empty)),
                Views = int.Parse(item["views"]),
                ActiveViewImpressions = int.Parse(item["activeViewViewableImpressions"]),
            };
        }

        private void SetFieldsForSummary(
            SearchDailySummary summary,
            Dictionary<string, string> item,
            SearchAccount searchAccount)
        {
            summary.Revenue = decimal.Parse(item["totalConvValue"]);
            summary.Cost = decimal.Parse(item["cost"]) / 1000000;
            var conversions = double.Parse(item["conversions"]);
            summary.Orders = Convert.ToInt32(conversions);
            summary.Clicks = int.Parse(item["clicks"]);
            summary.Impressions = int.Parse(item["impressions"]);
            summary.Device = DeviceStringToLetter(item["device"]);
            summary.ViewThrus = int.Parse(item["viewThroughConv"]);

            if (searchAccount.RevPerOrder.HasValue)
            {
                summary.Revenue = summary.Orders * searchAccount.RevPerOrder.Value;
            }
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
