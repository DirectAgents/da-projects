using CsvHelper.TypeConversion;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    /// <inheritdoc />
    /// <summary>
    /// VCD Report Converter for decimal values.
    /// </summary>
    internal abstract class DecimalReportConverter : StringConverter
    {
        /// <summary>
        /// Empty text value.
        /// </summary>
        protected const string EmptyTextValue = "—";

        /// <inheritdoc />
        /// <summary>
        /// Convert string value to decimal value.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="text">Text value.</param>
        /// <returns></returns>
        public override object ConvertFromString(TypeConverterOptions options, string text)
        {
            if (text == EmptyTextValue)
            {
                return GetValueWhenEmpty();
            }
            var preparedTextValue = PrepareTextValue(text);
            var convertedValue = ConvertValueToDecimal(preparedTextValue);
            var calculatedValue = CalculateValue(convertedValue);
            return calculatedValue;
        }

        /// <summary>
        /// Gets specified value when text value was empty.
        /// </summary>
        /// <returns>Specified value.</returns>
        protected abstract decimal GetValueWhenEmpty();

        /// <summary>
        /// Prepares text value for next steps.
        /// </summary>
        /// <param name="textValue">Text value.</param>
        /// <returns>Modified text value.</returns>
        protected abstract string PrepareTextValue(string textValue);

        /// <summary>
        /// Converts text value to decimal.
        /// </summary>
        /// <param name="value">Text value.</param>
        /// <returns>Converted decimal value.</returns>
        protected abstract decimal ConvertValueToDecimal(string value);

        /// <summary>
        /// Calculates decimal value.
        /// </summary>
        /// <param name="value">Decimal value.</param>
        /// <returns>Modified decimal value.</returns>
        protected abstract decimal CalculateValue(decimal value);
    }
}
