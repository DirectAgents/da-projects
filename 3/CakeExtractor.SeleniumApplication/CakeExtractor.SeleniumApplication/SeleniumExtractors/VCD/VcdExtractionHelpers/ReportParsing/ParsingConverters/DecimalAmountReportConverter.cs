using CsvHelper.TypeConversion;
using System.Globalization;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    internal sealed class DecimalAmountReportConverter : StringConverter
    {
        private const string EmptyValue = "—";

        public override object ConvertFromString(TypeConverterOptions options, string text)
        {
            if (text == EmptyValue)
            {
                return (decimal)0;
            }

            var d = decimal.Parse(text,
                NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | 
                NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands);
            return d;
        }
    }
}

