using System.Globalization;
using CsvHelper.TypeConversion;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    internal sealed class DecimalPercentageReportConverter : StringConverter
    {
        private const string EmptyValue = "—";
        private const decimal ValueForEmpty = -0.000001M;

        public override object ConvertFromString(TypeConverterOptions options, string text)
        {
            if (text == EmptyValue)
            {
                return ValueForEmpty;
            }
            var preparedTextValue = text.TrimEnd('%');
            var value = decimal.Parse(
                preparedTextValue,
                NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands);
            return value / 100M;
        }
    }
}
