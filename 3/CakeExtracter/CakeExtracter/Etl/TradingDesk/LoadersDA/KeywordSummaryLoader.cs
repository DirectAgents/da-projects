﻿using AutoMapper;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CakeExtracter.Common;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    internal class KeywordSummaryLoader : Loader<KeywordSummary>
    {
        public static EntityIdStorage<Keyword> DefaultKeywordStorage = StorageCollection.KeywordStorage;

        public readonly int AccountId;

        private readonly EntityIdStorage<Strategy> strategyStorage;
        private readonly EntityIdStorage<AdSet> adSetStorage;
        private readonly EntityIdStorage<Keyword> keywordStorage;
        private readonly TDStrategySummaryLoader strategyLoader;
        private readonly TDAdSetSummaryLoader adSetLoader;
        private readonly SummaryMetricLoader metricLoader;

        public KeywordSummaryLoader(int accountId = -1, EntityIdStorage<Keyword> keywordStorage = null,
            EntityIdStorage<AdSet> adSetStorage = null, EntityIdStorage<Strategy> strategyStorage = null)
        {
            this.strategyStorage = strategyStorage ?? TDStrategySummaryLoader.DefaultStrategyStorage;
            this.adSetStorage = adSetStorage ?? TDAdSetSummaryLoader.DefaultAdSetStorage;
            this.keywordStorage = keywordStorage ?? DefaultKeywordStorage;
            AccountId = accountId;
            strategyLoader = new TDStrategySummaryLoader(accountId, this.strategyStorage);
            adSetLoader = new TDAdSetSummaryLoader(accountId, this.adSetStorage, this.strategyStorage);
            metricLoader = new SummaryMetricLoader();
        }

        protected override int Load(List<KeywordSummary> items)
        {
            Logger.Info(AccountId, "Loading {0} DA-TD KeywordSummaries..", items.Count);
            PrepareData(items);
            AddUpdateDependentStrategies(items);
            AddUpdateDependentAdSets(items);
            AddUpdateDependentKeywords(items);
            AssignKeywordIdToItems(items);
            var count = UpsertDailySummaries(items);
            return count;
        }

        public void PrepareData(List<KeywordSummary> items)
        {
            items.ForEach(x =>
            {
                if (x.Keyword == null)
                {
                    x.Keyword = new Keyword();
                }
                Mapper.Map(x, x.Keyword);
            });
        }

        public void AssignKeywordIdToItems(List<KeywordSummary> items)
        {
            foreach (var item in items)
            {
                if (!keywordStorage.IsEntityInStorage(item.Keyword))
                {
                    continue;
                }
                item.KeywordId = keywordStorage.GetEntityIdFromStorage(item.Keyword);
                item.Keyword = null;
            }
        }

        public int UpsertDailySummaries(List<KeywordSummary> items)
        {
            var progress = new LoadingProgress();

            using (var db = new ClientPortalProgContext())
            {
                var itemKeywordIds = items.Select(i => i.KeywordId).Distinct().ToArray();
                var keywordIdsInDb = db.Keywords.Select(a => a.Id).Where(i => itemKeywordIds.Contains(i)).ToArray();

                foreach (var item in items)
                {
                    var target = db.Set<KeywordSummary>().Find(item.Date, item.KeywordId);
                    if (target == null)
                    {
                        TryToAddSummary(db, item, keywordIdsInDb, progress);
                    }
                    else
                    {
                        TryToUpdateSummary(db, item, target, progress);
                    }

                    progress.ItemCount++;

                }

                SafeContextWrapper.TrySaveChanges(db);
            }

            Logger.Info(AccountId, "Saving {0} KeywordSummaries ({1} updates, {2} additions, {3} duplicates, {4} deleted, {5} already-deleted, {6} skipped)",
                        progress.ItemCount, progress.UpdatedCount, progress.AddedCount, progress.DuplicateCount, progress.DeletedCount, progress.AlreadyDeletedCount, progress.SkippedCount);
            if (progress.DuplicateCount > 0)
            {
                Logger.Warn(AccountId, "Encountered {0} duplicates which were skipped", progress.DuplicateCount);
            }
            return progress.ItemCount;
        }

        public void AddUpdateDependentStrategies(List<KeywordSummary> items)
        {
            var strategies = items.Select(x => x.Keyword.Strategy).ToList();
            strategyLoader.AddUpdateDependentStrategies(strategies);
            AssignStrategyIdToItems(items);
        }

        public void AddUpdateDependentAdSets(List<KeywordSummary> items)
        {
            var adSets = items.Select(x => x.Keyword.AdSet).ToList();
            adSetLoader.AddUpdateDependentAdSets(adSets);
            AssignAdSetIdToItems(items);
        }

        public void AddUpdateDependentKeywords(List<KeywordSummary> items)
        {
            var keywords = items.Select(x => x.Keyword).ToList();
            AddUpdateDependentKeywords(keywords);
        }

        public void AddUpdateDependentKeywords(List<Keyword> items)
        {
            var notNullableItems = items.Where(x => x != null).ToList();
            notNullableItems.ForEach(x => x.AccountId = AccountId);
            var keywords = notNullableItems
                .Where(x => !string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.ExternalId))
                .GroupBy(x => new { x.AccountId, x.AdSetId, x.StrategyId, x.Name, x.ExternalId, x.MediaType })
                .Select(x => x.First())
                .ToList();
            AddUpdateDependentKeywordsInDb(keywords);
        }

        private void AddUpdateDependentKeywordsInDb(List<Keyword> items)
        {
            using (var db = new ClientPortalProgContext())
            {
                foreach (var keyword in items)
                {
                    if (keywordStorage.IsEntityInStorage(keyword))
                    {
                        continue;
                    }

                    SafeContextWrapper.Lock(SafeContextWrapper.KeywordLocker, () =>
                    {
                        var keywordsInDbList = GetKeywords(db, keyword);
                        if (!keywordsInDbList.Any())
                        {
                            var keywordInDb = AddKeyword(db, keyword);
                            Logger.Info(AccountId, "Saved new Keyword: {0} ({1}), ExternalId={2}", keywordInDb.Name, keywordInDb.Id, keywordInDb.ExternalId);
                            keywordStorage.AddEntityIdToStorage(keywordInDb);
                        }
                        else
                        {
                            var numUpdates = UpdateKeywords(db, keywordsInDbList, keyword);
                            if (numUpdates > 0)
                            {
                                Logger.Info(AccountId, "Updated Keyword: {0}, Eid={1}", keyword.Name, keyword.ExternalId);
                                if (numUpdates > 1)
                                {
                                    Logger.Warn(AccountId, "Multiple entities in db ({0})", numUpdates);
                                }
                            }
                            keywordStorage.AddEntityIdToStorage(keywordsInDbList.First());
                        }
                    });
                }
            }
        }

        private void AssignStrategyIdToItems(IEnumerable<KeywordSummary> items)
        {
            foreach (var item in items)
            {
                if (strategyStorage.IsEntityInStorage(item.Keyword.Strategy))
                {
                    item.Keyword.AdSet.StrategyId = strategyStorage.GetEntityIdFromStorage(item.Keyword.Strategy);
                    item.Keyword.StrategyId = strategyStorage.GetEntityIdFromStorage(item.Keyword.Strategy);
                }
                item.Keyword.Strategy = null;
            }
        }

        private void AssignAdSetIdToItems(IEnumerable<KeywordSummary> items)
        {
            foreach (var item in items)
            {
                if (adSetStorage.IsEntityInStorage(item.Keyword.AdSet))
                {
                    item.Keyword.AdSetId = adSetStorage.GetEntityIdFromStorage(item.Keyword.AdSet);
                }
                item.Keyword.AdSet = null;
            }
        }

        private void TryToAddSummary(ClientPortalProgContext db, KeywordSummary item, IEnumerable<int> keywordIdsInDb, LoadingProgress progress)
        {
            if (item.AllZeros())
            {
                progress.AlreadyDeletedCount++;
                return;
            }
            if (!keywordIdsInDb.Contains(item.KeywordId))
            {
                Logger.Warn(AccountId, "Skipping load of item. Keyword with id {0} does not exist.", item.KeywordId);
                progress.SkippedCount++;
                return;
            }
            db.KeywordSummaries.Add(item);
            UpsertSummaryMetrics(db, item);
            progress.AddedCount++;
        }

        private void TryToUpdateSummary(ClientPortalProgContext db, KeywordSummary item, KeywordSummary target, LoadingProgress progress)
        {
            var entry = db.Entry(target);
            if (entry.State != EntityState.Unchanged)
            {
                Logger.Warn(AccountId, "Encountered duplicate for {0:d} - Keyword {1}", item.Date, item.KeywordId);
                progress.DuplicateCount++;
                return;
            }

            if (item.AllZeros())
            {
                entry.State = EntityState.Deleted;
                progress.DeletedCount++;
                return;
            }
            entry.State = EntityState.Detached;
            Mapper.Map(item, target);
            entry.State = EntityState.Modified;
            UpsertSummaryMetrics(db, item);
            RemoveOldSummaryMetrics(db, item, target);
            progress.UpdatedCount++;
        }

        private void UpsertSummaryMetrics(ClientPortalProgContext db, KeywordSummary item)
        {
            item.InitialMetrics = GetItemMetrics(item);
            if (item.InitialMetrics == null || !item.InitialMetrics.Any())
            {
                return;
            }
            item.InitialMetrics.ForEach(x => x.EntityId = item.KeywordId);
            metricLoader.AddDependentMetricTypes(item.InitialMetrics);
            metricLoader.AssignMetricTypeIdToItems(item.InitialMetrics);
            metricLoader.UpsertSummaryMetrics<KeywordSummaryMetric>(db, item.InitialMetrics);
        }

        private IEnumerable<SummaryMetric> GetItemMetrics(KeywordSummary item)
        {
            var metrics = item.InitialMetrics == null
                ? item.Metrics
                : item.Metrics == null
                    ? item.InitialMetrics
                    : item.InitialMetrics.Concat(item.Metrics);
            return metrics?.ToList();
        }

        private void RemoveOldSummaryMetrics(ClientPortalProgContext db, KeywordSummary item, KeywordSummary target)
        {
            var deletedMetrics = item.InitialMetrics == null
                ? target.Metrics
                : target.Metrics.Where(x => item.InitialMetrics.All(m => m.MetricTypeId != x.MetricTypeId));
            metricLoader.RemoveMetrics(db, deletedMetrics);
        }

        private List<Keyword> GetKeywords(ClientPortalProgContext db, Keyword keyword)
        {
            IQueryable<Keyword> keywordsInDb;
            if (!string.IsNullOrWhiteSpace(keyword.ExternalId))
            {
                keywordsInDb = db.Keywords.Where(a => a.AccountId == keyword.AccountId && a.ExternalId == keyword.ExternalId);
                if (!keywordsInDb.Any())
                {
                    keywordsInDb = db.Keywords.Where(x => x.AccountId == keyword.AccountId && x.ExternalId == null && x.Name == keyword.Name);
                    keywordsInDb = FilterKeywordsByForeignKeys(keywordsInDb, keyword);
                }
            }
            else
            {
                keywordsInDb = db.Keywords.Where(x => x.AccountId == keyword.AccountId && x.Name == keyword.Name);
                keywordsInDb = FilterKeywordsByForeignKeys(keywordsInDb, keyword);
            }
            return keywordsInDb.ToList();
        }

        private IQueryable<Keyword> FilterKeywordsByForeignKeys(IQueryable<Keyword> keywords, Keyword keyword)
        {
            if (keyword.AdSetId.HasValue)
            {
                keywords = keywords.Where(x => x.AdSetId == keyword.AdSetId);
            }
            if (keyword.StrategyId.HasValue)
            {
                keywords = keywords.Where(x => x.StrategyId == keyword.StrategyId);
            }

            return keywords;
        }

        private Keyword AddKeyword(ClientPortalProgContext db, Keyword keywordProps)
        {
            var keyword = new Keyword
            {
                AccountId = keywordProps.AccountId,
                AdSetId = keywordProps.AdSetId,
                StrategyId = keywordProps.StrategyId,
                ExternalId = keywordProps.ExternalId,
                Name = keywordProps.Name,
                MediaType = keywordProps.MediaType,
            };
            db.Keywords.Add(keyword);
            SafeContextWrapper.TrySaveChanges(db);
            return keyword;
        }

        private int UpdateKeywords(ClientPortalProgContext db, IEnumerable<Keyword> keywordsInDbList, Keyword keywordProps)
        {
            foreach (var keyword in keywordsInDbList)
            {
                if (!string.IsNullOrWhiteSpace(keywordProps.ExternalId))
                {
                    keyword.ExternalId = keywordProps.ExternalId;
                }
                if (!string.IsNullOrWhiteSpace(keywordProps.Name))
                {
                    keyword.Name = keywordProps.Name;
                }
                if (keywordProps.AdSetId.HasValue)
                {
                    keyword.AdSetId = keywordProps.AdSetId;
                }
                if (keywordProps.StrategyId.HasValue)
                {
                    keyword.StrategyId = keywordProps.StrategyId;
                }
                if (!string.IsNullOrWhiteSpace(keywordProps.MediaType))
                {
                    keyword.MediaType = keywordProps.MediaType;
                }
            }
            return SafeContextWrapper.TrySaveChanges(db);
        }
    }
}
