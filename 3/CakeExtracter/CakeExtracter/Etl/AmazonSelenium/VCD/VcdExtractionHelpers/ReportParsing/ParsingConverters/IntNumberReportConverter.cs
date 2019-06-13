using CsvHelper.TypeConversion;
using System.Globalization;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    internal sealed class IntNumberReportConverter : StringConverter
    {
        private const string EmptyValue = "—";

        public override object ConvertFromString(TypeConverterOptions options, string text)
        {
            if (text == EmptyValue)
            {
                return 0;
            }

            var count = int.Parse(text, NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign);
            return count;
        }
    }
}
