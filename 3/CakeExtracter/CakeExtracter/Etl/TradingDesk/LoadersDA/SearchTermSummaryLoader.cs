using AutoMapper;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    internal class SearchTermSummaryLoader : Loader<SearchTermSummary>
    {
        public readonly int AccountId;
        public static EntityIdStorage<SearchTerm> SearchTermStorage;

        private readonly TDStrategySummaryLoader strategyLoader;
        private readonly TDAdSetSummaryLoader adSetLoader;
        private readonly KeywordSummaryLoader keywordLoader;
        private readonly EntityIdStorage<Strategy> strategyStorage;
        private readonly EntityIdStorage<AdSet> adSetStorage;
        private readonly EntityIdStorage<Keyword> keywordStorage;
        private readonly SummaryMetricLoader metricLoader;

        static SearchTermSummaryLoader()
        {
            SearchTermStorage = new EntityIdStorage<SearchTerm>(x => x.Id, x => $"{x.Name}{x.KeywordId}");
        }

        public SearchTermSummaryLoader(int accountId = -1)
        {
            AccountId = accountId;
            strategyLoader = new TDStrategySummaryLoader(accountId);
            adSetLoader = new TDAdSetSummaryLoader(accountId);
            keywordLoader = new KeywordSummaryLoader(accountId);
            strategyStorage = TDStrategySummaryLoader.StrategyStorage;
            adSetStorage = TDAdSetSummaryLoader.AdSetStorage;
            keywordStorage = KeywordSummaryLoader.KeywordStorage;
            metricLoader = new SummaryMetricLoader();
        }

        protected override int Load(List<SearchTermSummary> items)
        {
            Logger.Info(AccountId, "Loading {0} DA-TD SearchTermSummaries..", items.Count);
            PrepareData(items);
            AddUpdateDependentStrategies(items);
            AddUpdateDependentAdSets(items);
            AddUpdateDependentKeywords(items);
            AddUpdateDependentSearchTerms(items);
            AssignSearchTermIdToItems(items);
            var count = UpsertDailySummaries(items);
            return count;
        }

        public void PrepareData(List<SearchTermSummary> items)
        {
            items.ForEach(x =>
            {
                if (x.SearchTerm == null)
                {
                    x.SearchTerm = new SearchTerm();
                }
                Mapper.Map(x, x.SearchTerm);
                if (x.SearchTerm.Keyword == null)
                {
                    x.SearchTerm.Keyword = new Keyword();
                }
                Mapper.Map(x, x.SearchTerm.Keyword);
            });
        }

        public void AssignSearchTermIdToItems(List<SearchTermSummary> items)
        {
            foreach (var item in items)
            {
                if (SearchTermStorage.IsEntityInStorage(item.SearchTerm))
                {
                    item.SearchTermId = SearchTermStorage.GetEntityIdFromStorage(item.SearchTerm);
                    item.SearchTerm = null;
                }
            }
        }

        public int UpsertDailySummaries(List<SearchTermSummary> items)
        {
            var progress = new LoadingProgress();
            SafeContextWrapper.SaveChangedContext<ClientPortalProgContext>(
                SafeContextWrapper.SearchTermSummaryLocker, db =>
                {
                    var itemSearchTermIds = items.Select(i => i.SearchTermId).Distinct().ToArray();
                    var termIdsInDb = db.SearchTerms.Select(a => a.Id).Where(i => itemSearchTermIds.Contains(i))
                        .ToArray();

                    foreach (var item in items)
                    {
                        var target = db.Set<SearchTermSummary>().Find(item.Date, item.SearchTermId);
                        if (target == null)
                        {
                            TryToAddSummary(db, item, termIdsInDb, progress);
                        }
                        else
                        {
                            TryToUpdateSummary(db, item, target, progress);
                        }
                        progress.ItemCount++;
                    }
                }
            );

            Logger.Info(AccountId, "Saving {0} SearchTermSummaries ({1} updates, {2} additions, {3} duplicates, {4} deleted, {5} already-deleted, {6} skipped)",
                        progress.ItemCount, progress.UpdatedCount, progress.AddedCount, progress.DuplicateCount, progress.DeletedCount, progress.AlreadyDeletedCount, progress.SkippedCount);
            if (progress.DuplicateCount > 0)
            {
                Logger.Warn(AccountId, "Encountered {0} duplicates which were skipped", progress.DuplicateCount);
            }
            return progress.ItemCount;
        }

        public void AddUpdateDependentStrategies(List<SearchTermSummary> items)
        {
            var strategies = items
                .GroupBy(i => new { i.SearchTerm.Keyword.Strategy?.Name, i.SearchTerm.Keyword.Strategy?.ExternalId })
                .Select(x => x.First().SearchTerm.Keyword.Strategy)
                .Where(x => x != null && (!string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.ExternalId)))
                .ToList();
            strategyLoader.AddUpdateDependentStrategies(strategies);
            AssignStrategyIdToItems(items);
        }

        public void AddUpdateDependentAdSets(List<SearchTermSummary> items)
        {
            var adSets = items
                .GroupBy(x => new { x.SearchTerm.Keyword.AdSet?.StrategyId, x.SearchTerm.Keyword.AdSet?.ExternalId, x.SearchTerm.Keyword.AdSet?.Name })
                .Select(x => x.First().SearchTerm.Keyword.AdSet)
                .Where(x => x != null && (!string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.ExternalId)))
                .ToList();
            adSetLoader.AddUpdateDependentAdSets(adSets, AccountId);
            AssignAdSetIdToItems(items);
        }

        public void AddUpdateDependentKeywords(List<SearchTermSummary> items)
        {
            var keywords = items
                .GroupBy(x => new { x.SearchTerm.Keyword.AdSetId, x.SearchTerm.Keyword.StrategyId, x.SearchTerm.Keyword.Name, x.SearchTerm.Keyword.ExternalId })
                .Select(x => x.First().SearchTerm.Keyword)
                .Where(x => x != null && (!string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.ExternalId)))
                .ToList();
            keywordLoader.AddUpdateDependentKeywords(keywords);
            AssignKeywordIdToItems(items);
        }

        public void AddUpdateDependentSearchTerms(List<SearchTermSummary> items)
        {
            var searchTerms = items
                .GroupBy(i => new { i.SearchTerm.KeywordId, i.SearchTerm.Name })
                .Select(x => x.First().SearchTerm)
                .ToList();
            AddUpdateDependentSearchTerms(searchTerms);
        }

        private void AddUpdateDependentSearchTerms(List<SearchTerm> items)
        {
            using (var db = new ClientPortalProgContext())
            {
                foreach (var terms in items)
                {
                    if (SearchTermStorage.IsEntityInStorage(terms))
                    {
                        continue;
                    }

                    SafeContextWrapper.Lock(SafeContextWrapper.SearchTermLocker, () =>
                    {
                        var termsInDbList = GetSearchTerms(db, terms, AccountId);
                        if (!termsInDbList.Any())
                        {
                            var termsInDb = AddSearchTerm(db, terms, AccountId);
                            Logger.Info(AccountId, "Saved new SearchTerm: {0} ({1}), KeywordId={2}", termsInDb.Name, termsInDb.Id, termsInDb.KeywordId);
                            SearchTermStorage.AddEntityIdToStorage(termsInDb);
                        }
                        else
                        {
                            var numUpdates = UpdateSearchTerms(db, termsInDbList, terms);
                            if (numUpdates > 0)
                            {
                                Logger.Info(AccountId, "Updated SearchTerm: {0}, KeywordId={1}", terms.Name, terms.KeywordId);
                                if (numUpdates > 1)
                                {
                                    Logger.Warn(AccountId, "Multiple entities in db ({0})", numUpdates);
                                }
                            }
                            SearchTermStorage.AddEntityIdToStorage(termsInDbList.First());
                        }
                    });
                }
            }
        }

        private void AssignStrategyIdToItems(List<SearchTermSummary> items)
        {
            foreach (var item in items)
            {
                if (strategyStorage.IsEntityInStorage(item.SearchTerm.Keyword.Strategy))
                {
                    item.SearchTerm.Keyword.StrategyId = strategyStorage.GetEntityIdFromStorage(item.SearchTerm.Keyword.Strategy);
                }
                item.SearchTerm.Keyword.Strategy = null;
            }
        }

        private void AssignAdSetIdToItems(List<SearchTermSummary> items)
        {
            foreach (var item in items)
            {
                if (adSetStorage.IsEntityInStorage(item.SearchTerm.Keyword.AdSet))
                {
                    item.SearchTerm.Keyword.AdSetId = adSetStorage.GetEntityIdFromStorage(item.SearchTerm.Keyword.AdSet);
                }
                item.SearchTerm.Keyword.AdSet = null;
            }
        }

        private void AssignKeywordIdToItems(List<SearchTermSummary> items)
        {
            foreach (var item in items)
            {
                if (keywordStorage.IsEntityInStorage(item.SearchTerm.Keyword))
                {
                    item.SearchTerm.KeywordId = keywordStorage.GetEntityIdFromStorage(item.SearchTerm.Keyword);
                }
                item.SearchTerm.Keyword = null;
            }
        }

        private void TryToAddSummary(ClientPortalProgContext db, SearchTermSummary item, IEnumerable<int> termIdsInDb, LoadingProgress progress)
        {
            if (item.AllZeros())
            {
                progress.AlreadyDeletedCount++;
                return;
            }
            if (!termIdsInDb.Contains(item.SearchTermId))
            {
                Logger.Warn(AccountId, "Skipping load of item. SearchTerm with id {0} does not exist.", item.SearchTermId);
                progress.SkippedCount++;
                return;
            }
            db.SearchTermSummaries.Add(item);
            UpsertSummaryMetrics(db, item);
            progress.AddedCount++;
        }

        private void TryToUpdateSummary(ClientPortalProgContext db, SearchTermSummary item, SearchTermSummary target, LoadingProgress progress)
        {
            var entry = db.Entry(target);
            if (entry.State != EntityState.Unchanged)
            {
                Logger.Warn(AccountId, "Encountered duplicate for {0:d} - SearchTerm {1}", item.Date, item.SearchTermId);
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

        private void UpsertSummaryMetrics(ClientPortalProgContext db, SearchTermSummary item)
        {
            item.InitialMetrics = GetItemMetrics(item);
            if (item.InitialMetrics == null || !item.InitialMetrics.Any())
            {
                return;
            }
            item.InitialMetrics.ForEach(x => x.EntityId = item.SearchTermId);
            metricLoader.AddDependentMetricTypes(item.InitialMetrics);
            metricLoader.AssignMetricTypeIdToItems(item.InitialMetrics);
            metricLoader.UpsertSummaryMetrics<SearchTermSummaryMetric>(db, item.InitialMetrics);
        }

        private IEnumerable<SummaryMetric> GetItemMetrics(SearchTermSummary item)
        {
            var metrics = item.InitialMetrics == null
                ? item.Metrics
                : item.Metrics == null
                    ? item.InitialMetrics
                    : item.InitialMetrics.Concat(item.Metrics);
            return metrics?.ToList();
        }

        private void RemoveOldSummaryMetrics(ClientPortalProgContext db, SearchTermSummary item, SearchTermSummary target)
        {
            var deletedMetrics = item.InitialMetrics == null
                ? target.Metrics
                : target.Metrics.Where(x => !item.InitialMetrics.Any(m => m.MetricTypeId == x.MetricTypeId));
            metricLoader.RemoveMetrics(db, deletedMetrics);
        }

        private List<SearchTerm> GetSearchTerms(ClientPortalProgContext db, SearchTerm term, int accountId)
        {
            IQueryable<SearchTerm> termsInDb;
            if (term.KeywordId.HasValue)
            {
                termsInDb = db.SearchTerms.Where(a => a.AccountId == AccountId && a.KeywordId == term.KeywordId && a.Name == term.Name);
                if (!termsInDb.Any())
                {
                    termsInDb = db.SearchTerms.Where(x => x.AccountId == accountId && x.KeywordId == null && x.Name == term.Name);
                }
            }
            else
            {
                termsInDb = db.SearchTerms.Where(x => x.AccountId == accountId && x.Name == term.Name);
            }
            return termsInDb.ToList();
        }

        private SearchTerm AddSearchTerm(ClientPortalProgContext db, SearchTerm termProps, int accountId)
        {
            var term = new SearchTerm
            {
                AccountId = accountId,
                KeywordId = termProps.KeywordId,
                Name = termProps.Name
            };
            db.SearchTerms.Add(term);
            SafeContextWrapper.TrySaveChanges(db);
            return term;
        }

        private int UpdateSearchTerms(ClientPortalProgContext db, IEnumerable<SearchTerm> termsInDbList, SearchTerm termProps)
        {
            foreach (var term in termsInDbList)
            {
                if (termProps.KeywordId.HasValue)
                {
                    term.KeywordId = termProps.KeywordId;
                }
                if (!string.IsNullOrWhiteSpace(termProps.Name))
                {
                    term.Name = termProps.Name;
                }
            }
            return SafeContextWrapper.TrySaveChanges(db);
        }
    }
}
