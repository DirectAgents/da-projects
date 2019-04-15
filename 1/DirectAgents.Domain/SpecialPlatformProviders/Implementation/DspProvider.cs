using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.SpecialPlatformProviders.Contracts;

namespace DirectAgents.Domain.SpecialPlatformProviders.Implementation
{
    public class DspProvider : SpecialPlatformProvider
    {
        public DspProvider() : base(Platform.Code_DspAmazon)
        {
        }

        public override IEnumerable<SpecialPlatformSummary> GetDatesRangeByAccounts(ClientPortalProgContext context)
        {
            var dspSummaries = GetSummariesGroupedByAccountId(context).ToList();
            AssignExtAccountForSummaries(dspSummaries, context);
            return dspSummaries;
        }

        private static IEnumerable<SpecialPlatformSummary> GetSummariesGroupedByAccountId(
            ClientPortalProgContext context)
        {
            return context.DspCreativesMetricValues
                .GroupBy(x => x.Creative.AccountId)
                .Select(x => new SpecialPlatformSummary
                {
                    AccountId = x.Key,
                    EarliestDate = x.Min(z => z.Date),
                    LatestDate = x.Max(z => z.Date)
                });
        }
    }
}
