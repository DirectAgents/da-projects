using System.Globalization;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    /// <inheritdoc />
    /// <summary>
    /// VCD Report Converter for decimal values for monetary amounts.
    /// </summary>
    internal sealed class DecimalAmountReportConverter : DecimalReportConverter
    {
        /// <inheritdoc />
        protected override decimal GetValueWhenEmpty()
        {
            return 0M;
        }

        /// <inheritdoc />
        protected override string PrepareTextValue(string textValue)
        {
            return textValue;
        }

        /// <inheritdoc />
        protected override decimal ConvertValueToDecimal(string value)
        {
            return decimal.Parse(value, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands);
        }

        /// <inheritdoc />
        protected override decimal CalculateValue(decimal value)
        {
            return value;
        }
    }
}