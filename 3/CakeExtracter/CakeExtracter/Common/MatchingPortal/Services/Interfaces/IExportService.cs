using System.Collections.Generic;
using System.Threading.Tasks;
using CakeExtracter.Common.MatchingPortal.Models;

namespace CakeExtracter.Common.MatchingPortal.Services.Interfaces
{
    /// <summary>
    /// Service to export data frames into various report formats
    /// </summary>
    public interface IExportService
    {
        /// <summary>
        /// Exports a data frame with a given filter.
        /// </summary>
        /// <param name="reportColumnProviders">Column extractors used to extract data to show in the report.</param>
        /// <param name="filter">Filter to use when exporting data or null if no filtering should be applied.</param>
        /// <returns>A task of constructed report.</returns>
        Task<DataFrameExport> ExportDataFrame(IEnumerable<ReportColumnProvider> reportColumnProviders, ResultFilter filter = null);
    }
}
