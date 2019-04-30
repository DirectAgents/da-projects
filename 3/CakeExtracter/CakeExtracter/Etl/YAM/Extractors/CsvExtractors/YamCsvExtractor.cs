using System.IO;
using CakeExtracter.Common.Extractors.CsvExtractors;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowMaps;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using CsvHelper;

namespace CakeExtracter.Etl.YAM.Extractors.CsvExtractors
{
    internal class YamCsvExtractor : CsvAccountExtractor<YamRow, YamRowMap>
    {
        private const string ErrorMessageIfReportIsEmpty = "No header record was found.";

        public YamCsvExtractor(int accountId, string summariesName, StreamReader streamReader) : base(accountId,
            summariesName, streamReader)
        {
            ReadingExceptionCallback += exception =>
            {
                if (exception is CsvHelperException && exception.Message == ErrorMessageIfReportIsEmpty)
                {
                    Logger.Warn(accountId, $"There are no statistics in the report: {exception.Message}");
                }
            };
        }
    }
}