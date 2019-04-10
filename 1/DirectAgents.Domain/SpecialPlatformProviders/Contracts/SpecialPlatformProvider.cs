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

        public abstract IQueryable<SpecialPlatformSummary> GetDatesRangeByAccounts(ClientPortalProgContext context);

        protected IQueryable<ExtAccount> GetAccountsByPlatform(ClientPortalProgContext context)
        {
            return context.ExtAccounts.AsQueryable().Where(a => a.Platform.Code == PlatformCode);
        }
    }
}
