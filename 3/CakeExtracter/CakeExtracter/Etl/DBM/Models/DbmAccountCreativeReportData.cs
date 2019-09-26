using System.Collections.Generic;
using CakeExtracter.Etl.DBM.Extractors.Parsers.Models;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.DBM.Models
{
    /// <summary>
    /// Model for creative report data grouped by account.
    /// </summary>
    public class DbmAccountCreativeReportData
    {
        /// <summary>
        /// Gets or sets the external account.
        /// </summary>
        public ExtAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the list of creative report rows.
        /// </summary>
        public IEnumerable<DbmCreativeReportRow> CreativeReportRows { get; set; }
    }
}