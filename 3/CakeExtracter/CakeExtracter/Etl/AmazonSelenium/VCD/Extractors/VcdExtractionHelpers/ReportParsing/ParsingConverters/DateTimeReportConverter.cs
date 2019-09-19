using System;
using System.Globalization;
using CsvHelper.TypeConversion;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    internal sealed class DateTimeReportConverter : StringConverter
    {
        private const string EmptyValue = "—";
        private const string DateTimeReportFormat = "yyyy-MM-dd";

        public override object ConvertFromString(TypeConverterOptions options, string text)
        {
            if (text == EmptyValue)
            {
                return null;
            }
            return DateTime.ParseExact(text, DateTimeReportFormat, CultureInfo.InvariantCulture);
        }
    }
}
