using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.DBM.Extractors.Parsers.Models;
using CakeExtracter.Etl.DBM.Models;
using DBM.Parsers.Models;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.DBM.Composer
{
    /// <summary>
    /// Composer of data for DBM reports
    /// </summary>
    public class DbmReportDataComposer
    {
        private readonly List<ExtAccount> accounts;

        public DbmReportDataComposer(List<ExtAccount> accounts)
        {
            this.accounts = accounts;
        }

        /// <summary>
        /// The method groups a list of line item report rows by required accounts.
        /// </summary>
        /// <param name="reportRows">List of line item report rows</param>
        /// <returns>List of line item report data grouped by accounts</returns>
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

        /// <summary>
        /// The method groups a list of creative report rows by required accounts.
        /// </summary>
        /// <param name="reportRows">List of creative report rows</param>
        /// <returns>List of creative report data grouped by accounts</returns>
        public List<DbmAccountCreativeReportData> ComposeCreativeReportData(IEnumerable<DbmCreativeReportRow> reportRows)
        {
            var summariesGroupedByAccount = reportRows
                .GroupBy(re => re.AdvertiserId, (key, gr) =>
                {
                    var relatedAccount = accounts.FirstOrDefault(ac => ac.ExternalId == key);
                    return relatedAccount != null
                        ? new DbmAccountCreativeReportData
                        {
                            Account = relatedAccount,
                            CreativeReportRows = gr
                        }
                        : null;
                })
                .Where(data => true)
                .Where(reportData => reportData?.CreativeReportRows != null)
                .ToList();
            return summariesGroupedByAccount;
        }
    }
}
