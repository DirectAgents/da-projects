using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.DBM.Models;
using DBM.Parsers.Models;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.DBM.Composer
{
    public class DbmReportDataComposer
    {
        private readonly List<ExtAccount> accounts;

        public DbmReportDataComposer(List<ExtAccount> accounts)
        {
            this.accounts = accounts;
        }

        public List<DbmAccountReportData> Compose(IEnumerable<DbmCreativeReportRow> creativeReportRows)
        {
            var creativeSummariesGroupedByAccount = creativeReportRows
                .GroupBy(re => re.AdvertiserId, (key, gr) =>
                {
                    var relatedAccount = accounts.FirstOrDefault(ac => ac.ExternalId == key);
                    return relatedAccount != null
                        ? new DbmAccountReportData
                        {
                            Account = relatedAccount,
                            Data = gr
                        }
                        : null;
                })
                .Where(data => true)
                .Where(creativeReportData => creativeReportData?.Data != null)
                .ToList();
            return creativeSummariesGroupedByAccount;
        }
    }
}
