using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPSearch;

namespace DirectAgents.Domain.Concrete
{
    public partial class CPSearchRepository : ICPSearchRepository, IDisposable
    {
        private readonly ClientPortalSearchContext context;

        public CPSearchRepository(ClientPortalSearchContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public IQueryable<SearchProfile> SearchProfiles(DateTime? activeSince = null, bool includeGauges = false)
        {
            var searchProfiles = context.SearchProfiles.AsQueryable();
            if (activeSince.HasValue)
            {
                // First, filter out SPs that don't have any dailysummaries
                searchProfiles = searchProfiles.Where(sp => sp.SearchAccounts.Any(sa =>
                    sa.SearchCampaigns.Any(sc => sc.SearchDailySummaries.Any(ss => ss.Date >= activeSince.Value))));
            }

            if (includeGauges)
            {
                SetGauges(searchProfiles);
            }

            return searchProfiles;
        }

        public SearchProfile GetSearchProfile(int id)
        {
            return context.SearchProfiles.Find(id);
        }

        public bool SaveSearchProfile(SearchProfile searchProfile, bool saveIfExists = true, bool createIfDoesntExist = false)
        {
            if (context.SearchProfiles.Any(x => x.SearchProfileId == searchProfile.SearchProfileId))
            {
                if (saveIfExists)
                {
                    var entry = context.Entry(searchProfile);
                    entry.State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
            }
            else if (createIfDoesntExist)
            {
                context.SearchProfiles.Add(searchProfile);
                context.SaveChanges();
                return true;
            }
            return false; // not saved/created
        }

        public IEnumerable<SearchAccount> StatsGaugesByChannel()
        {
            var channels = context.SearchAccounts.Select(sa => sa.Channel).Distinct().OrderBy(ch => ch).ToList();
            var allSds = DailySummaries().Where(x => x.SearchCampaign.SearchAccountId.HasValue);
            var allScs = ConvSummaries().Where(x => x.SearchCampaign.SearchAccountId.HasValue);
            var allCds = CallSummaries().Where(x => x.SearchCampaign.SearchAccountId.HasValue);
            var searchAccounts = channels.Select(x => CreateAccountWithStatsGauge(x, allSds, allScs, allCds)).ToList();
            return searchAccounts;
        }

        public IQueryable<SearchAccount> SearchAccounts(int? spId = null, string channel = null,
            bool includeGauges = false)
        {
            var searchAccounts = context.SearchAccounts.AsQueryable();
            if (spId.HasValue)
            {
                searchAccounts = searchAccounts.Where(x => x.SearchProfileId == spId.Value);
            }
        
            if (!string.IsNullOrWhiteSpace(channel))
            {
                searchAccounts = searchAccounts.Where(x => x.Channel == channel);
            }

            if (includeGauges)
            {
                SetGauges(searchAccounts);
            }

            return searchAccounts;
        }

        public SearchAccount GetSearchAccount(int id)
        {
            return context.SearchAccounts.Find(id);
        }

        public bool SaveSearchAccount(SearchAccount searchAccount, bool createIfDoesntExist = false)
        {
            if (context.SearchAccounts.Any(sa => sa.SearchAccountId == searchAccount.SearchAccountId))
            {
                var entry = context.Entry(searchAccount);
                entry.State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }

            if (!createIfDoesntExist)
            {
                return false; // not saved/created
            }

            context.SearchAccounts.Add(searchAccount);
            context.SaveChanges();
            return true;
        }

        public IQueryable<SearchCampaign> SearchCampaigns(int? spId = null, int? searchAccountId = null,
            string channel = null, bool includeGauges = false)
        {
            var sc = context.SearchCampaigns.AsQueryable();
            if (searchAccountId.HasValue)
            {
                sc = sc.Where(x => x.SearchAccountId == searchAccountId.Value);
            }

            if (spId.HasValue)
            {
                sc = sc.Where(x => x.SearchAccount.SearchProfileId == spId.Value);
            }

            if (!string.IsNullOrWhiteSpace(channel))
            {
                sc = sc.Where(x => x.SearchAccount.Channel == channel);
            }

            if (includeGauges)
            {
                SetGauges(sc);
            }

            return sc;
        }

        public SearchCampaign GetSearchCampaign(int id)
        {
            return context.SearchCampaigns.Find(id);
        }

        public IQueryable<SearchDailySummary> DailySummaries(int? spId = null, int? searchAccountId = null,
            int? searchCampaignId = null)
        {
            var sds = context.SearchDailySummaries.AsQueryable();
            if (searchCampaignId.HasValue)
            {
                sds = sds.Where(x => x.SearchCampaignId == searchCampaignId.Value);
            }

            if (searchAccountId.HasValue)
            {
                sds = sds.Where(x => x.SearchCampaign.SearchAccountId == searchAccountId.Value);
            }

            if (spId.HasValue)
            {
                sds = sds.Where(x => x.SearchCampaign.SearchAccount.SearchProfileId == spId.Value);
            }

            return sds;
        }

        public IQueryable<SearchConvSummary> ConvSummaries(int? spId = null, int? searchAccountId = null,
            int? searchCampaignId = null, int? searchConvTypeId = null)
        {
            var scs = context.SearchConvSummaries.AsQueryable();
            if (searchCampaignId.HasValue)
            {
                scs = scs.Where(x => x.SearchCampaignId == searchCampaignId.Value);
            }

            if (searchConvTypeId.HasValue)
            {
                scs = scs.Where(x => x.SearchConvTypeId == searchConvTypeId.Value);
            }

            if (searchAccountId.HasValue)
            {
                scs = scs.Where(x => x.SearchCampaign.SearchAccountId == searchAccountId.Value);
            }

            if (spId.HasValue)
            {
                scs = scs.Where(x => x.SearchCampaign.SearchAccount.SearchProfileId == spId.Value);
            }

            return scs;
        }

        public IQueryable<CallDailySummary> CallSummaries(int? spId = null, int? searchAccountId = null,
            int? searchCampaignId = null)
        {
            var cds = context.CallDailySummaries.AsQueryable();
            if (searchCampaignId.HasValue)
            {
                cds = cds.Where(x => x.SearchCampaignId == searchCampaignId.Value);
            }

            if (searchAccountId.HasValue)
            {
                cds = cds.Where(x => x.SearchCampaign.SearchAccountId == searchAccountId.Value);
            }

            if (spId.HasValue)
            {
                cds = cds.Where(x => x.SearchCampaign.SearchAccount.SearchProfileId == spId.Value);
            }

            return cds;
        }

        public IEnumerable<SearchConvType> GetConvTypes(int? spId = null, int? searchAccountId = null,
            int? searchCampaignId = null, bool includeGauges = false)
        {
            var convSums = ConvSummaries(spId, searchAccountId, searchCampaignId);
            var convTypeIds = convSums.Select(x => x.SearchConvTypeId).Distinct().ToArray();
            var convTypes = context.SearchConvTypes.Where(x => convTypeIds.Contains(x.SearchConvTypeId));
            if (includeGauges)
            {
                SetGauges(convTypes, convSums);
            }

            return convTypes;
        }

        public SearchConvType GetConvType(int id)
        {
            var searchConvType = context.SearchConvTypes.Find(id);
            return searchConvType;
        }

        private SearchAccount CreateAccountWithStatsGauge(string channel, IQueryable<SearchDailySummary> allSds,
            IQueryable<SearchConvSummary> allScs, IQueryable<CallDailySummary> allCds)
        {
            var searchAccount = new SearchAccount { Channel = channel };
            var sds = allSds.Where(x => x.SearchCampaign.SearchAccount.Channel == channel);
            var scs = allScs.Where(x => x.SearchCampaign.SearchAccount.Channel == channel);
            var cds = allCds.Where(x => x.SearchCampaign.SearchAccount.Channel == channel);
            SetGauges(searchAccount, sds, scs, cds);
            return searchAccount;
        }

        private void SetGauges(IEnumerable<SearchConvType> searchConvTypes, IQueryable<SearchConvSummary> convSums)
        {
            foreach (var ct in searchConvTypes)
            {
                var scs = convSums.Where(x => x.SearchConvTypeId == ct.SearchConvTypeId);
                var any = scs.Any();
                ct.MinConvSum = any ? scs.Min(x => x.Date) : (DateTime?)null;
                ct.MaxConvSum = any ? scs.Max(x => x.Date) : (DateTime?)null;
            }
        }

        private void SetGauges(IEnumerable<SearchAccount> searchAccounts)
        {
            foreach (var sa in searchAccounts)
            {
                var sds = DailySummaries(searchAccountId: sa.SearchAccountId);
                var scs = ConvSummaries(searchAccountId: sa.SearchAccountId);
                var cds = CallSummaries(searchAccountId: sa.SearchAccountId);
                SetGauges(sa, sds, scs, cds);
            }
        }

        private void SetGauges(IEnumerable<SearchProfile> searchProfiles)
        {
            foreach (var sp in searchProfiles)
            {
                var sds = DailySummaries(spId: sp.SearchProfileId);
                var scs = ConvSummaries(spId: sp.SearchProfileId);
                var cds = CallSummaries(spId: sp.SearchProfileId);
                SetGauges(sp, sds, scs, cds);
            }
        }

        private void SetGauges(IEnumerable<SearchCampaign> searchCampaigns)
        {
            foreach (var sc in searchCampaigns)
            {
                var sds = DailySummaries(searchCampaignId: sc.SearchCampaignId);
                var scs = ConvSummaries(searchCampaignId: sc.SearchCampaignId);
                var cds = CallSummaries(searchCampaignId: sc.SearchCampaignId);
                SetGauges(sc, sds, scs, cds);
            }
        }

        private void SetGauges(ISearchGauge searchGauge, IQueryable<SearchDailySummary> sds,
            IQueryable<SearchConvSummary> scs, IQueryable<CallDailySummary> cds)
        {
            if (sds != null && sds.Any())
            {
                searchGauge.MinDaySum = sds.Min(x => x.Date);
                searchGauge.MaxDaySum = sds.Max(x => x.Date);
            }

            if (scs != null && scs.Any())
            {
                searchGauge.MinConvSum = scs.Min(x => x.Date);
                searchGauge.MaxConvSum = scs.Max(x => x.Date);
            }

            if (cds != null && cds.Any())
            {
                searchGauge.MinCallSum = cds.Min(x => x.Date);
                searchGauge.MaxCallSum = cds.Max(x => x.Date);
            }
        }

        // ---

        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
