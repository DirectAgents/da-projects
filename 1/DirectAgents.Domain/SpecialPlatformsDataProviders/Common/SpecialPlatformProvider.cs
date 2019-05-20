using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.SpecialPlatformProviders.Contracts
{
    public abstract class SpecialPlatformProvider
    {
        public string PlatformCode { get; }

        protected SpecialPlatformProvider(string platformCode)
        {
            PlatformCode = platformCode;
        }

        public abstract IEnumerable<SpecialPlatformLatestsSummary> GetDatesRangeByAccounts(ClientPortalProgContext context);
        
        protected void AssignExtAccountForSummaries(List<SpecialPlatformLatestsSummary> summaries, ClientPortalProgContext context)
        {
            summaries.ForEach(summary => summary.Account = GetAccountById(context, summary.AccountId));
        }

        private ExtAccount GetAccountById(ClientPortalProgContext context, int? id)
        {
            return context.ExtAccounts.FirstOrDefault(a => a.Id == id);
        }
    }
}
