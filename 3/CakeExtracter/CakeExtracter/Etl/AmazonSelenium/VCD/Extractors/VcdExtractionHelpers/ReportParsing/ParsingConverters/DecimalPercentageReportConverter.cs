using System.Globalization;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    /// <inheritdoc />
    /// <summary>
    /// VCD Report Converter for decimal values for monetary amounts.
    /// </summary>
    internal sealed class DecimalPercentageReportConverter : DecimalReportConverter
    {
        private const char PercentSymbol = '%';
        private const decimal ValueForEmpty = -0.000001M;

        /// <inheritdoc />
        protected override decimal GetValueWhenEmpty()
        {
            return ValueForEmpty;
        }

        /// <inheritdoc />
        protected override string PrepareTextValue(string textValue)
        {
            return textValue.TrimEnd(PercentSymbol);
        }

        /// <inheritdoc />
        protected override decimal ConvertValueToDecimal(string value)
        {
            return decimal.Parse(value, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands);
        }

        /// <inheritdoc />
        protected override decimal CalculateValue(decimal value)
        {
            return value / 100M;
        }
    }
}
