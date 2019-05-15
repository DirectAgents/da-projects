using CakeExtracter.Common.Extractors.CsvExtractors;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowMaps;
using CakeExtracter.Etl.YAM.Extractors.CsvExtractors.RowModels;
using CsvHelper;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.YAM.Extractors.CsvExtractors
{
    public class YamCsvExtractor : CsvAccountExtractor<YamRow, YamRowMap>
    {
        private const string ErrorMessageIfReportIsEmpty = "No header record was found.";

        public YamCsvExtractor(ExtAccount account) : base(account)
        {
            ReadingExceptionCallback += exception =>
            {
                if (exception is CsvHelperException && exception.Message == ErrorMessageIfReportIsEmpty)
                {
                    Logger.Warn(AccountId, $"There are no statistics in the report: {exception.Message}");
                }
            };
        }

        protected override bool ShouldSkipRecord(string[] fields)
        {
            //if the first column is “Totals” the current record is a record with information and should be skipped.
            return fields[0] == "Totals";
        }
    }
}