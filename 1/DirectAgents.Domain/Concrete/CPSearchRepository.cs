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
            var allSearchDailySummaries = DailySummaries().Where(x => x.SearchCampaign.SearchAccountId.HasValue);
            var allSearchConvSummaries = ConvSummaries().Where(x => x.SearchCampaign.SearchAccountId.HasValue);
            var allSearchCallSummeries = CallSummaries().Where(x => x.SearchCampaign.SearchAccountId.HasValue);
            var allVideoDailySummaries = VideoDailySummaries().Where(x => x.SearchCampaign.SearchAccountId.HasValue);
            var searchAccounts = channels.Select(x => CreateAccountWithStatsGauge(
                x,
                allSearchDailySummaries,
                allSearchConvSummaries,
                allSearchCallSummeries,
                allVideoDailySummaries)).ToList();
            return searchAccounts;
        }

        public IQueryable<SearchAccount> SearchAccounts(
            int? searchProfileId = null,
            string channel = null,
            bool includeGauges = false)
        {
            var searchAccounts = context.SearchAccounts.AsQueryable();
            if (searchProfileId.HasValue)
            {
                searchAccounts = searchAccounts.Where(x => x.SearchProfileId == searchProfileId.Value);
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

        public IQueryable<SearchCampaign> SearchCampaigns(
            int? searchProfileId = null,
            int? searchAccountId = null,
            string channel = null,
            bool includeGauges = false)
        {
            var searchCampaigns = context.SearchCampaigns.AsQueryable();
            if (searchAccountId.HasValue)
            {
                searchCampaigns = searchCampaigns.Where(x => x.SearchAccountId == searchAccountId.Value);
            }

            if (searchProfileId.HasValue)
            {
                searchCampaigns = searchCampaigns.Where(x => x.SearchAccount.SearchProfileId == searchProfileId.Value);
            }

            if (!string.IsNullOrWhiteSpace(channel))
            {
                searchCampaigns = searchCampaigns.Where(x => x.SearchAccount.Channel == channel);
            }

            if (includeGauges)
            {
                SetGauges(searchCampaigns);
            }

            return searchCampaigns;
        }

        public SearchCampaign GetSearchCampaign(int id)
        {
            return context.SearchCampaigns.Find(id);
        }

        public IQueryable<SearchDailySummary> DailySummaries(
            int? searchProfileId = null,
            int? searchAccountId = null,
            int? searchCampaignId = null)
        {
            var searchDailySummaries = context.SearchDailySummaries.AsQueryable();
            if (searchCampaignId.HasValue)
            {
                searchDailySummaries = searchDailySummaries.Where(x => x.SearchCampaignId == searchCampaignId.Value);
            }

            if (searchAccountId.HasValue)
            {
                searchDailySummaries = searchDailySummaries.Where(x => x.SearchCampaign.SearchAccountId == searchAccountId.Value);
            }

            if (searchProfileId.HasValue)
            {
                searchDailySummaries = searchDailySummaries.Where(x => x.SearchCampaign.SearchAccount.SearchProfileId == searchProfileId.Value);
            }

            return searchDailySummaries;
        }

        public IQueryable<SearchConvSummary> ConvSummaries(
            int? searchProfileId = null,
            int? searchAccountId = null,
            int? searchCampaignId = null,
            int? searchConvTypeId = null)
        {
            var searchConvSummaries = context.SearchConvSummaries.AsQueryable();
            if (searchCampaignId.HasValue)
            {
                searchConvSummaries = searchConvSummaries.Where(x => x.SearchCampaignId == searchCampaignId.Value);
            }

            if (searchConvTypeId.HasValue)
            {
                searchConvSummaries = searchConvSummaries.Where(x => x.SearchConvTypeId == searchConvTypeId.Value);
            }

            if (searchAccountId.HasValue)
            {
                searchConvSummaries = searchConvSummaries.Where(x => x.SearchCampaign.SearchAccountId == searchAccountId.Value);
            }

            if (searchProfileId.HasValue)
            {
                searchConvSummaries = searchConvSummaries.Where(x => x.SearchCampaign.SearchAccount.SearchProfileId == searchProfileId.Value);
            }

            return searchConvSummaries;
        }

        public IQueryable<CallDailySummary> CallSummaries(
            int? searchProfileId = null,
            int? searchAccountId = null,
            int? searchCampaignId = null)
        {
            var callDailySummaries = context.CallDailySummaries.AsQueryable();
            if (searchCampaignId.HasValue)
            {
                callDailySummaries = callDailySummaries.Where(x => x.SearchCampaignId == searchCampaignId.Value);
            }

            if (searchAccountId.HasValue)
            {
                callDailySummaries = callDailySummaries.Where(x => x.SearchCampaign.SearchAccountId == searchAccountId.Value);
            }

            if (searchProfileId.HasValue)
            {
                callDailySummaries = callDailySummaries.Where(x => x.SearchCampaign.SearchAccount.SearchProfileId == searchProfileId.Value);
            }

            return callDailySummaries;
        }

        public IQueryable<SearchVideoDailySummary> VideoDailySummaries(
            int? searchProfileId = null,
            int? searchAccountId = null,
            int? searchCampaignId = null)
        {
            var videoDailySummaries = context.SearchVideoDailySummaries.AsQueryable();
            if (searchCampaignId.HasValue)
            {
                videoDailySummaries = videoDailySummaries.Where(x => x.SearchCampaign.SearchCampaignId == searchCampaignId.Value);
            }

            if (searchAccountId.HasValue)
            {
                videoDailySummaries = videoDailySummaries.Where(x => x.SearchCampaign.SearchAccountId == searchAccountId.Value);
            }

            if (searchProfileId.HasValue)
            {
                videoDailySummaries = videoDailySummaries.Where(x => x.SearchCampaign.SearchAccount.SearchProfileId == searchProfileId.Value);
            }

            return videoDailySummaries;
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

        private SearchAccount CreateAccountWithStatsGauge(
            string channel,
            IQueryable<SearchDailySummary> allSearchDailySummaries,
            IQueryable<SearchConvSummary> allSearchConvSummaries,
            IQueryable<CallDailySummary> allCallDailySummaries,
            IQueryable<SearchVideoDailySummary> allVideoDailySummaries)
        {
            var searchAccount = new SearchAccount { Channel = channel };
            var searchDailySummaries = allSearchDailySummaries.Where(x => x.SearchCampaign.SearchAccount.Channel == channel);
            var searchConvSummaries = allSearchConvSummaries.Where(x => x.SearchCampaign.SearchAccount.Channel == channel);
            var callDailySummaries = allCallDailySummaries.Where(x => x.SearchCampaign.SearchAccount.Channel == channel);
            var videoDailySummaries = allVideoDailySummaries.Where(x => x.SearchCampaign.SearchAccount.Channel == channel);
            SetGauges(searchAccount, searchDailySummaries, searchConvSummaries, callDailySummaries, videoDailySummaries);
            return searchAccount;
        }

        private void SetGauges(IEnumerable<SearchConvType> searchConvTypes, IQueryable<SearchConvSummary> convSums)
        {
            foreach (var ct in searchConvTypes)
            {
                var searchConvSummaries = convSums.Where(x => x.SearchConvTypeId == ct.SearchConvTypeId);
                var any = searchConvSummaries.Any();
                ct.MinConvSum = any ? searchConvSummaries.Min(x => x.Date) : (DateTime?)null;
                ct.MaxConvSum = any ? searchConvSummaries.Max(x => x.Date) : (DateTime?)null;
            }
        }

        private void SetGauges(IEnumerable<SearchAccount> searchAccounts)
        {
            foreach (var searchAccount in searchAccounts)
            {
                var searchDailySummaries = DailySummaries(searchAccountId: searchAccount.SearchAccountId);
                var searchConvSummaries = ConvSummaries(searchAccountId: searchAccount.SearchAccountId);
                var callDailySummaries = CallSummaries(searchAccountId: searchAccount.SearchAccountId);
                var videoDailySummaries = VideoDailySummaries(searchAccountId: searchAccount.SearchAccountId);
                SetGauges(searchAccount, searchDailySummaries, searchConvSummaries, callDailySummaries, videoDailySummaries);
            }
        }

        private void SetGauges(IEnumerable<SearchProfile> searchProfiles)
        {
            foreach (var searchProfile in searchProfiles)
            {
                var searchDailySummaries = DailySummaries(searchProfileId: searchProfile.SearchProfileId);
                var searchConvSummaries = ConvSummaries(searchProfileId: searchProfile.SearchProfileId);
                var callDailySummaries = CallSummaries(searchProfileId: searchProfile.SearchProfileId);
                var videoDailySummaries = VideoDailySummaries(searchProfileId: searchProfile.SearchProfileId);
                SetGauges(searchProfile, searchDailySummaries, searchConvSummaries, callDailySummaries, videoDailySummaries);
            }
        }

        private void SetGauges(IEnumerable<SearchCampaign> searchCampaigns)
        {
            foreach (var searchCampaign in searchCampaigns)
            {
                var searchDailySummaries = DailySummaries(searchCampaignId: searchCampaign.SearchCampaignId);
                var searchConvSummaries = ConvSummaries(searchCampaignId: searchCampaign.SearchCampaignId);
                var callDailySummaries = CallSummaries(searchCampaignId: searchCampaign.SearchCampaignId);
                var videoDailySummmaries = VideoDailySummaries(searchCampaignId: searchCampaign.SearchCampaignId);
                SetGauges(searchCampaign, searchDailySummaries, searchConvSummaries, callDailySummaries, videoDailySummmaries);
            }
        }

        private void SetGauges(
            ISearchGauge searchGauge,
            IQueryable<SearchDailySummary> searchDailySummaries,
            IQueryable<SearchConvSummary> searchConvSummaries,
            IQueryable<CallDailySummary> callDailySummaries,
            IQueryable<SearchVideoDailySummary> videoDailySummaries)
        {
            if (searchDailySummaries != null && searchDailySummaries.Any())
            {
                searchGauge.MinDaySum = searchDailySummaries.Min(x => x.Date);
                searchGauge.MaxDaySum = searchDailySummaries.Max(x => x.Date);
            }

            if (searchConvSummaries != null && searchConvSummaries.Any())
            {
                searchGauge.MinConvSum = searchConvSummaries.Min(x => x.Date);
                searchGauge.MaxConvSum = searchConvSummaries.Max(x => x.Date);
            }

            if (callDailySummaries != null && callDailySummaries.Any())
            {
                searchGauge.MinCallSum = callDailySummaries.Min(x => x.Date);
                searchGauge.MaxCallSum = callDailySummaries.Max(x => x.Date);
            }

            if (videoDailySummaries != null && videoDailySummaries.Any())
            {
                searchGauge.MinVidSum = videoDailySummaries.Min(x => x.Date);
                searchGauge.MaxVidSum = videoDailySummaries.Max(x => x.Date);
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
