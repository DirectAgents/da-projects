using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.DBM.Extractors.Parsers.Models;
using CakeExtracter.Etl.DBM.Models;
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

        public List<DbmAccountLineItemReportData> ComposeLineItemReportData(IEnumerable<DbmLineItemReportRow> reportRows)
        {
            var summariesGroupedByAccount = reportRows
                .GroupBy(re => re.AdvertiserId, (key, gr) =>
                {
                    var relatedAccount = accounts.FirstOrDefault(ac => ac.ExternalId == key);
                    return relatedAccount != null
                        ? new DbmAccountLineItemReportData
                        {
                            Account = relatedAccount,
                            LineItemReportRows = gr
                        }
                        : null;
                })
                .Where(data => true)
                .Where(reportData => reportData?.LineItemReportRows != null)
                .ToList();
            return summariesGroupedByAccount;
        }
    }
}
