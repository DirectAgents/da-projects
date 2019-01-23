using CsvHelper.TypeConversion;
using System.Globalization;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    internal sealed class DecimalAmountReportConverter : StringConverter
    {
        public DecimalAmountReportConverter()
        {
        }

        public override object ConvertFromString(TypeConverterOptions options, string text)
        {
            if (text == "—")
            {
                return (decimal)0;
            }
            if (!string.IsNullOrEmpty(text) && text[0] == '$')
                text = text.Remove(0, 1);
            decimal d = decimal.Parse(text, CultureInfo.InvariantCulture);
            return d;
        }

    }
}

