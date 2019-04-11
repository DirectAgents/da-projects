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

        public override IQueryable<SpecialPlatformSummary> GetDatesRangeByAccounts(ClientPortalProgContext context)
        {
            var dspAccounts = GetAccountsByPlatform(context);
            var dspSummaries = dspAccounts.Select(account => new SpecialPlatformSummary
            {
                Account = account,
                EarliestDate = context.DspCreativesMetricValues
                    .Where(x => x.Creative.AccountId == account.Id)
                    .Select(x => x.Date)
                    .Min(),
                LatestDate = context.DspCreativesMetricValues
                    .Where(x => x.Creative.AccountId == account.Id)
                    .Select(x => x.Date)
                    .Max()
            });
            return dspSummaries;
        }
    }
}
