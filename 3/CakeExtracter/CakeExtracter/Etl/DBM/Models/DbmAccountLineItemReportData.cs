using System.Collections.Generic;
using CakeExtracter.Etl.DBM.Extractors.Parsers.Models;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.DBM.Models
{
    /// <summary>
    /// Model for line item report data grouped by account.
    /// </summary>
    public class DbmAccountLineItemReportData
    {
        /// <summary>
        /// Gets or sets the external account.
        /// </summary>
        public ExtAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the list of line item report rows.
        /// </summary>
        public IEnumerable<DbmLineItemReportRow> LineItemReportRows { get; set; }
    }
}