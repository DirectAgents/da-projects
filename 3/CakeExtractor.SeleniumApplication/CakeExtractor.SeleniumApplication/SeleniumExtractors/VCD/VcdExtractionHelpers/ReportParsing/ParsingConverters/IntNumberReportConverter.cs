using CsvHelper.TypeConversion;
using System.Globalization;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    internal sealed class IntNumberReportConverter : StringConverter
    {
        public IntNumberReportConverter()
        {
        }

        public override object ConvertFromString(TypeConverterOptions options, string text)
        {
            if (text == "—")
            {
                return 0;
            }
            int count = int.Parse(text, NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture); ;
            return count;
        }
    }
}
