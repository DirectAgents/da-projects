using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformProviders.Contracts;

namespace DirectAgents.Domain.SpecialPlatformProviders.Implementation
{
    public class KochavaProvider : SpecialPlatformProvider
    {
        public KochavaProvider() : base(Platform.Code_Kochava)
        {
        }

        public override IEnumerable<SpecialPlatformSummary> GetDatesRangeByAccounts(ClientPortalProgContext context)
        {
            var dbSummaries = GetDbSummariesGroupedByAccountId(context);
            var kochavaSummaries = GetSummariesGroupedByAccount(dbSummaries);
            AssignExtAccountForSummaries(kochavaSummaries, context);
            return kochavaSummaries;
        }

        private static IQueryable<SpecialPlatformSummaryDb> GetDbSummariesGroupedByAccountId(
            ClientPortalProgContext context)
        {
            return context.KochavaItems
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
