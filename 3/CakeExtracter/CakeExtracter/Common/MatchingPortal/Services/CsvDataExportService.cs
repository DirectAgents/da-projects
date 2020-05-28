using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CakeExtracter.Common.MatchingPortal.Models;
using CakeExtracter.Common.MatchingPortal.Services.Interfaces;

namespace CakeExtracter.Common.MatchingPortal.Services
{
    /// <inheritdoc />
    public class CsvDataExportService : IExportService
    {
        private const string ContentType = "text/csv";

        private IFilterService _filterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataExportService"/> class.
        /// </summary>
        /// <param name="filterService">Service used to apply filters to the match data</param>
        public CsvDataExportService(IFilterService filterService)
        {
            _filterService = filterService;
        }

        /// <inheritdoc/>
        public Task<DataFrameExport> ExportDataFrame(ResultFilter filter = null)
        {
            var dataFrame = new DataFrameExport() { ContentType = ContentType, Timestamp = DateTime.UtcNow };
            var items = _filterService.ApplyResultsFilter(filter ?? new ResultFilter());
            dataFrame.Content = BuildCsvFile(items);
            return Task.FromResult(dataFrame);
        }

        private byte[] BuildCsvFile(IReadOnlyCollection<MatchResult> items)
        {
            using (var buffer = new MemoryStream())
            {
                using (var writer = new StreamWriter(buffer))
                {
                    writer.WriteLine("product_id,new_product_title,product_description,product_updated_date");
                    foreach (var item in items)
                    {
                        writer.WriteLine($"{item.ProductId},{item.NewProductTitle},{item.ProductDescription},{item.MatchingDate}");
                    }
                    writer.Flush();
                    buffer.Seek(0, SeekOrigin.Begin);
                    return buffer.GetBuffer();
                }
            }
        }
    }
}
