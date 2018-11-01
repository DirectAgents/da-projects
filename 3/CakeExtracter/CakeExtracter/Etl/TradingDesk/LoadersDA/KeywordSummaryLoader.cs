using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using CakeExtracter.Etl.TradingDesk.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    class KeywordSummaryLoader : Loader<KeywordSummary>
    {
        public readonly int AccountId;
        public static EntityIdStorage<Keyword> KeywordStorage;

        private readonly TDStrategySummaryLoader strategyLoader;
        private readonly TDAdSetSummaryLoader adSetLoader;
        private readonly EntityIdStorage<Strategy> strategyStorage;
        private readonly EntityIdStorage<AdSet> adSetStorage;
        private readonly SummaryMetricLoader metricLoader;

        static KeywordSummaryLoader()
        {
            KeywordStorage = new EntityIdStorage<Keyword>(x => x.Id, x => $"{x.AdSetId}{x.StrategyId}{x.Name}{x.ExternalId}", x => $"{x.Name}{x.ExternalId}");
        }

        public KeywordSummaryLoader(int accountId = -1)
        {
            AccountId = accountId;
            strategyLoader = new TDStrategySummaryLoader(accountId);
            adSetLoader = new TDAdSetSummaryLoader(accountId);
            strategyStorage = TDStrategySummaryLoader.StrategyStorage;
            adSetStorage = TDAdSetSummaryLoader.AdSetStorage;
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
                if (KeywordStorage.IsEntityInStorage(item.Keyword))
                {
                    item.KeywordId = KeywordStorage.GetEntityIdFromStorage(item.Keyword);
                    item.Keyword = null;
                }
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
                Logger.Info(AccountId, "Saving {0} KeywordSummaries ({1} updates, {2} additions, {3} duplicates, {4} deleted, {5} already-deleted, {6} skipped)",
                            progress.ItemCount, progress.UpdatedCount, progress.AddedCount, progress.DuplicateCount, progress.DeletedCount, progress.AlreadyDeletedCount, progress.SkippedCount);
                if (progress.DuplicateCount > 0)
                {
                    Logger.Warn(AccountId, "Encountered {0} duplicates which were skipped", progress.DuplicateCount);
                }
                var numChanges = db.SaveChanges();
            }
            return progress.ItemCount;
        }

        public void AddUpdateDependentStrategies(List<KeywordSummary> items)
        {
            var strategies = items
                .GroupBy(i => new { i.Keyword.Strategy?.Name, i.Keyword.Strategy?.ExternalId })
                .Select(x => x.First().Keyword.Strategy)
                .Where(x => x != null && (!string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.ExternalId)))
                .ToList();
            strategyLoader.AddUpdateDependentStrategies(strategies);
            AssignStrategyIdToItems(items);
        }

        public void AddUpdateDependentAdSets(List<KeywordSummary> items)
        {
            var adSets = items
                .GroupBy(x => new { x.Keyword.AdSet?.StrategyId, x.Keyword.AdSet?.ExternalId, x.Keyword.AdSet?.Name })
                .Select(x => x.First().Keyword.AdSet)
                .Where(x => x != null && (!string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.ExternalId)))
                .ToList();
            adSetLoader.AddUpdateDependentAdSets(adSets, AccountId);
            AssignAdSetIdToItems(items);
        }

        public void AddUpdateDependentKeywords(List<KeywordSummary> items)
        {
            var keywords = items
                .GroupBy(i => new { i.Keyword.AdSetId, i.Keyword.StrategyId, i.Keyword.Name, i.Keyword.ExternalId })
                .Select(x => x.First().Keyword)
                .ToList();
            AddUpdateDependentKeywords(keywords);
        }

        private void AddUpdateDependentKeywords(List<Keyword> items)
        {
            using (var db = new ClientPortalProgContext())
            {
                foreach (var keyword in items)
                {
                    if (KeywordStorage.IsEntityInStorage(keyword))
                    {
                        continue;
                    }

                    var keywordsInDbList = GetKeywords(db, keyword, AccountId);
                    if (!keywordsInDbList.Any())
                    {
                        var keywordInDb = AddKeyword(db, keyword, AccountId);
                        Logger.Info(AccountId, "Saved new Keyword: {0} ({1}), ExternalId={2}", keywordInDb.Name, keywordInDb.Id, keywordInDb.ExternalId);
                        KeywordStorage.AddEntityIdToStorage(keywordInDb);
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
                        KeywordStorage.AddEntityIdToStorage(keywordsInDbList.First());
                    }
                }
            }
        }

        private void AssignStrategyIdToItems(List<KeywordSummary> items)
        {
            foreach (var item in items)
            {
                if (strategyStorage.IsEntityInStorage(item.Keyword.Strategy))
                {
                    item.Keyword.StrategyId = strategyStorage.GetEntityIdFromStorage(item.Keyword.Strategy);
                }
                item.Keyword.Strategy = null;
            }
        }

        private void AssignAdSetIdToItems(List<KeywordSummary> items)
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
            progress.UpdatedCount++;
        }

        private void UpsertSummaryMetrics(ClientPortalProgContext db, KeywordSummary item)
        {
            if (item.Metrics == null || !item.Metrics.Any())
            {
                return;
            }
            item.Metrics.ForEach(x => x.EntityId = item.KeywordId);
            metricLoader.AddDependentMetricTypes(item.Metrics);
            metricLoader.AssignMetricTypeIdToItems(item.Metrics);
            metricLoader.UpsertSummaryMetrics<KeywordSummaryMetric>(db, item.Metrics);
        }

        private List<Keyword> GetKeywords(ClientPortalProgContext db, Keyword keyword, int accountId)
        {
            IQueryable<Keyword> keywordsInDb;
            if (!string.IsNullOrWhiteSpace(keyword.ExternalId))
            {
                keywordsInDb = db.Keywords.Where(a => a.AccountId == AccountId && a.ExternalId == keyword.ExternalId);
                if (!keywordsInDb.Any())
                {
                    keywordsInDb = db.Keywords.Where(x => x.AccountId == accountId && x.ExternalId == null && x.Name == keyword.Name);
                    keywordsInDb = FilterKeywordsByForeignKeys(keywordsInDb, keyword);
                }
            }
            else
            {
                keywordsInDb = db.Keywords.Where(x => x.AccountId == accountId && x.Name == keyword.Name);
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

        private Keyword AddKeyword(ClientPortalProgContext db, Keyword keywordProps, int accountId)
        {
            var keyword = new Keyword
            {
                AccountId = accountId,
                AdSetId = keywordProps.AdSetId,
                StrategyId = keywordProps.StrategyId,
                ExternalId = keywordProps.ExternalId,
                Name = keywordProps.Name
            };
            db.Keywords.Add(keyword);
            db.SaveChanges();
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
            }
            return db.SaveChanges();
        }
    }
}
