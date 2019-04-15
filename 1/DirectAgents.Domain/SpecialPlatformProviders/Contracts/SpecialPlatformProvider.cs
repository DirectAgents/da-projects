using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Domain.SpecialPlatformProviders.Contracts
{
    public abstract class SpecialPlatformProvider
    {
        public string PlatformCode { get; }

        protected SpecialPlatformProvider(string platformCode)
        {
            PlatformCode = platformCode;
        }

        public abstract IEnumerable<SpecialPlatformSummary> GetDatesRangeByAccounts(ClientPortalProgContext context);
        
        protected void AssignExtAccountForSummaries(List<SpecialPlatformSummary> summaries, ClientPortalProgContext context)
        {
            summaries.ForEach(summary => summary.Account = GetAccountById(context, summary.AccountId));
        }

        private static ExtAccount GetAccountById(ClientPortalProgContext context, int? id)
        {
            return context.ExtAccounts.FirstOrDefault(a => a.Id == id);
        }
    }
}
