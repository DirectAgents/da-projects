using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformProviders.Contracts;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.SpecialPlatformProviders.Implementation
{
    public class KochavaProvider : SpecialPlatformProvider
    {
        public KochavaProvider() : base(Platform.Code_Kochava)
        {
        }

        public override IEnumerable<SpecialPlatformLatestsSummary> GetDatesRangeByAccounts(ClientPortalProgContext context)
        {
            var kochavaSummaries = GetSummariesGroupedByAccountId(context).ToList();
            AssignExtAccountForSummaries(kochavaSummaries, context);
            return kochavaSummaries;
        }

        private IEnumerable<SpecialPlatformLatestsSummary> GetSummariesGroupedByAccountId(
            ClientPortalProgContext context)
        {
            return context.KochavaItems
                .GroupBy(x => x.AccountId)
                .Select(x => new SpecialPlatformLatestsSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date)
                });
        }
    }
}
