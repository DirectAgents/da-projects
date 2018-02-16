using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.CPSearch;

namespace DirectAgents.Domain.Concrete
{
    public class CPSearchRepository : ICPSearchRepository, IDisposable
    {
        private ClientPortalSearchContext context;

        public CPSearchRepository(ClientPortalSearchContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public IQueryable<SearchProfile> SearchProfiles()
        {
            return context.SearchProfiles;
        }

        public IQueryable<SearchAccount> SearchAccounts(int? spId = null, bool includeGauges = false)
        {
            var searchAccounts = context.SearchAccounts.AsQueryable();
            if (spId.HasValue)
                searchAccounts = searchAccounts.Where(x => x.SearchProfileId == spId.Value);
            if (includeGauges)
                searchAccounts = SetGauges(searchAccounts).AsQueryable();
            return searchAccounts;
        }
        public IEnumerable<SearchAccount> SetGauges(IEnumerable<SearchAccount> searchAccounts)
        {
            foreach (var sa in searchAccounts)
            {
                var sds = DailySummaries(searchAccountId: sa.SearchAccountId);
                bool any = sds.Any();
                sa.MinDaySum = any ? sds.Min(x => x.Date) : (DateTime?)null;
                sa.MaxDaySum = any ? sds.Max(x => x.Date) : (DateTime?)null;
                var scs = ConvSummaries(searchAccountId: sa.SearchAccountId);
                any = scs.Any();
                sa.MinConvSum = any ? scs.Min(x => x.Date) : (DateTime?)null;
                sa.MaxConvSum = any ? scs.Max(x => x.Date) : (DateTime?)null;
            }
            return searchAccounts;
        }

        public SearchAccount GetSearchAccount(int id)
        {
            return context.SearchAccounts.Find(id);
        }

        public IQueryable<SearchDailySummary> DailySummaries(int? spId = null, int? searchAccountId = null)
        {
            var sds = context.SearchDailySummaries.AsQueryable();
            if (spId.HasValue)
            {
                var campIds = context.SearchProfiles.Where(x => x.SearchProfileId == spId.Value).SelectMany(x => x.SearchAccounts)
                                .SelectMany(x => x.SearchCampaigns).Select(x => x.SearchCampaignId).ToArray();
                sds = sds.Where(x => campIds.Contains(x.SearchCampaignId));

                // not sure if this would work or produce efficient sql (2 foreign keys are nullable):
                //sds = sds.Where(x => x.SearchCampaign.SearchAccount.SearchProfileId == spId.Value);
            }
            if (searchAccountId.HasValue)
            {
                var campIds = context.SearchAccounts.Where(x => x.SearchAccountId == searchAccountId.Value)
                                .SelectMany(x => x.SearchCampaigns).Select(x => x.SearchCampaignId).ToArray();
                sds = sds.Where(x => campIds.Contains(x.SearchCampaignId));
            }
            return sds;
        }

        public IQueryable<SearchConvSummary> ConvSummaries(int? spId = null, int? searchAccountId = null)
        {
            var scs = context.SearchConvSummaries.AsQueryable();
            if (spId.HasValue)
            {
                var campIds = context.SearchProfiles.Where(x => x.SearchProfileId == spId.Value).SelectMany(x => x.SearchAccounts)
                                .SelectMany(x => x.SearchCampaigns).Select(x => x.SearchCampaignId).ToArray();
                scs = scs.Where(x => campIds.Contains(x.SearchCampaignId));
            }
            if (searchAccountId.HasValue)
            {
                var campIds = context.SearchAccounts.Where(x => x.SearchAccountId == searchAccountId.Value)
                                .SelectMany(x => x.SearchCampaigns).Select(x => x.SearchCampaignId).ToArray();
                scs = scs.Where(x => campIds.Contains(x.SearchCampaignId));
            }
            return scs;
        }

        // ---

        public IQueryable<ClientReport> ClientReports()
        {
            return context.ClientReports;
        }

        // ---

        private bool disposed = false;

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
