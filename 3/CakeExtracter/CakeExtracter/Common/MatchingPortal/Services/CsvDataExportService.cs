using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using CakeExtracter.Common.MatchingPortal.Models;
using CakeExtracter.Common.MatchingPortal.Services.Interfaces;

namespace CakeExtracter.Common.MatchingPortal.Services
{
    /// <inheritdoc />
    public class CsvDataExportService : IExportService
    {
        private const string ContentType = "text/csv";

        private const string CsvDelimiter = ",";

        private readonly IFilterService _filterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataExportService"/> class.
        /// </summary>
        /// <param name="filterService">Service used to apply filters to the match data.</param>
        public CsvDataExportService(IFilterService filterService)
        {
            _filterService = filterService;
        }

        /// <inheritdoc/>
        public Task<DataFrameExport> ExportDataFrame(IReadOnlyCollection<ReportColumnProvider> reportColumnProviders, ResultFilter filter = null)
        {
            var dataFrame = new DataFrameExport() { ContentType = ContentType, Timestamp = DateTime.UtcNow };
            var items = _filterService.ApplyResultsFilter(filter ?? new ResultFilter());
            dataFrame.Content = BuildCsvFile(items, reportColumnProviders);
            return Task.FromResult(dataFrame);
        }

        private string SafelyExtractValue(MatchResult item, ReportColumnProvider provider)
        {
            try
            {
                var data = provider.ValueExtractor(item);
                return data.Replace(",", string.Empty);
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                return string.Empty;
            }
        }

        private byte[] BuildCsvFile(IEnumerable<MatchResult> items, IReadOnlyCollection<ReportColumnProvider> reportColumnProviders)
        {
            using (var buffer = new MemoryStream())
            {
                using (var writer = new StreamWriter(buffer))
                {
                    writer.WriteLine(string.Join(CsvDelimiter, reportColumnProviders.Select(p => p.ColumnName)));
                    foreach (var item in items)
                    {
                        writer.WriteLine(string.Join(CsvDelimiter, reportColumnProviders.Select(p => SafelyExtractValue(item, p))));
                    }
                    writer.Flush();
                    buffer.Seek(0, SeekOrigin.Begin);
                    return buffer.GetBuffer();
                }
            }
        }
    }
}
