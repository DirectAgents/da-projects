using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.CPSearch;

namespace DirectAgents.Domain.Abstract
{
    public interface ICPSearchRepository : IDisposable
    {
        void SaveChanges();

        IQueryable<SearchProfile> SearchProfiles(DateTime? activeSince = null);
        IQueryable<SearchAccount> SearchAccounts(int? spId = null, bool includeGauges = false);
        SearchAccount GetSearchAccount(int id);

        IQueryable<SearchDailySummary> DailySummaries(int? spId = null, int? searchAccountId = null);
        IQueryable<SearchConvSummary> ConvSummaries(int? spId = null, int? searchAccountId = null);

        IQueryable<ClientReport> ClientReports();
    }
}
