using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformProviders.Contracts;

namespace DirectAgents.Domain.SpecialPlatformProviders.Implementation
{
    public class VcdProvider : SpecialPlatformProvider
    {
        public VcdProvider() : base(Platform.Code_AraAmazon)
        {
        }

        public override IEnumerable<SpecialPlatformSummary> GetDatesRangeByAccounts(ClientPortalProgContext context)
        {
            var dbSummaries = GetDbSummariesGroupedByAccountId(context);
            var vcdSummaries = GetSummariesGroupedByAccount(dbSummaries);
            AssignExtAccountForSummaries(vcdSummaries, context);
            return vcdSummaries;   
        }

        private static IQueryable<SpecialPlatformSummaryDb> GetDbSummariesGroupedByAccountId(
            ClientPortalProgContext context)
        {
            return context.VcdAnalytic
                .GroupBy(x => x.AccountId)
                .Select(x => new SpecialPlatformSummaryDb
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date)
                });
        }
    }
}
